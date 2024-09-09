class WwiseAudioWorkletProcessor extends AudioWorkletProcessor {
	constructor(nodeOptions) {
		super();

		this._initialized = false;
		this.port.onmessage = this._onmessage.bind(this);
	}

	_onmessage(event) {
		const evtdata = event.data;
		if (!this.validate(evtdata)) {
			console.error("Invalid data sent to initialize WwiseWorkletProcessor!");
			return;
		}

		this._options = evtdata;

		this._numBuffers = evtdata.numbuffers;
		this._bufferLength = evtdata.bufferlen;
		this._channelCount = evtdata.channelcount;
		this._readIndex = 0; // Index of full buffer-length reads
		this._frameIndex = 0; // Read index within frame; buffer length is expected to be a multiple of 128.

		this._initialized = true;
	}

	validate(data) {
		if (!data.buffer) return false;
		if (!data.state) return false;
		if (!data.bufferlen || data.bufferlen < 0) return false;
		if (!data.numbuffers || data.numbuffers < 0) return false;
		if (!data.channelcount || data.channelCount <= 0) return false;

		return true;
	}

	process(inputs, outputs) {

		if (!this._initialized) {
			return true;
		}

		// These memory views must be re-constructed on each callback, in case the WASM memory heap grows
		var ringBufferState = new Int32Array(this._options.state, 0, this._options.fields.NUM_FIELDS);
		const ringBufferData = new Float32Array(this._options.buffer, 0, this._bufferLength * this._channelCount * this._numBuffers);

		if (outputs[0].length != this._channelCount) {
			console.error("WwiseAudioWorkletProcessor: Expected " + this._channelCount + " channels, got " + outputs[0].length + "; no processing will take place.");
			return true;
		}

		var buffersReady = ringBufferState[this._options.fields.BUFFERS_READY];
		if (buffersReady > 0)
		{
			// Perform the copy
			const inlen = this._bufferLength;
			const outlen = outputs[0][0].length;

			if (outlen > (inlen - this._frameIndex)) {
				console.error("WwiseAudioWorkletProcessor: Wwise frame size ", inlen, " is not a multiple of WebAudio frame size ", outlen);
			}

			for (var c = 0; c < this._channelCount; c++) {
				const outputChannelData = outputs[0][c];
				if (outputChannelData.length != outlen) {
					console.error("WwiseAudioWorkletProcessor: Expected frame size ", outlen, ", got ", outputChannelData.length);
				}
				const channelOffset = this._readIndex + (c * inlen) + this._frameIndex;
				const inputChannelData = ringBufferData.subarray(channelOffset, channelOffset + outlen);
				outputChannelData.set(inputChannelData);
			}

			this._frameIndex += outlen;
			if (this._frameIndex >= inlen) {
				Atomics.sub(ringBufferState, this._options.fields.BUFFERS_READY, 1);
				this._readIndex = (this._readIndex + inlen * this._channelCount) % (inlen * this._channelCount * this._numBuffers);
				this._frameIndex = 0;
			}
		}
		else
		{
			// Output buffers are already filled with zeros, so no need to fill them again
			// Signal the sound engine that we have voice starvation
			Atomics.compareExchange(ringBufferState, this._options.fields.STARVING, 0, 1);
		}

		return true;
	}
}

registerProcessor('wwise-worklet-processor', WwiseAudioWorkletProcessor);
