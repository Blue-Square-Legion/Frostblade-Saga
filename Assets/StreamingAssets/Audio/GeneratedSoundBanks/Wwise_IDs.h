/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_AMBIENCE = 278617630U;
        static const AkUniqueID PLAY_BIGSWORDDRAG = 516602709U;
        static const AkUniqueID PLAY_BIGSWORDHIT = 3959107170U;
        static const AkUniqueID PLAY_BIGSWORDSWING = 1761942167U;
        static const AkUniqueID PLAY_BOSSBATTLE = 153638151U;
        static const AkUniqueID PLAY_BOSSBITE = 2729937127U;
        static const AkUniqueID PLAY_BOSSPROJECTILE = 2050525748U;
        static const AkUniqueID PLAY_BOSSPROJECTILEHIT = 42433307U;
        static const AkUniqueID PLAY_BOSSTIGERROAR = 3232321306U;
        static const AkUniqueID PLAY_CHECKPOINT = 2962822744U;
        static const AkUniqueID PLAY_COMBAT = 513571230U;
        static const AkUniqueID PLAY_ENEMYATTACKCRY = 923355438U;
        static const AkUniqueID PLAY_ENEMYFIREBALL = 162522213U;
        static const AkUniqueID PLAY_ENEMYHITCRY = 3101974183U;
        static const AkUniqueID PLAY_ENEMYIDLECRY = 1710694676U;
        static const AkUniqueID PLAY_FIRETIGERATTACKROAR = 1310873743U;
        static const AkUniqueID PLAY_FIRETIGERBITE = 978409677U;
        static const AkUniqueID PLAY_FIRETIGERFOOTSTEPS = 1980244072U;
        static const AkUniqueID PLAY_FIRETIGERHURTROAR = 201807964U;
        static const AkUniqueID PLAY_FIRETIGERIDLEROAR = 3798153045U;
        static const AkUniqueID PLAY_HEARTBEAT = 3765695918U;
        static const AkUniqueID PLAY_ICEAURA = 268692114U;
        static const AkUniqueID PLAY_ICEAURAFREEZE = 1516146351U;
        static const AkUniqueID PLAY_PLAYER_ATTACK_VOX = 606945166U;
        static const AkUniqueID PLAY_PLAYER_DIE_VOX = 2344515640U;
        static const AkUniqueID PLAY_PLAYER_HIT_VOX = 1388893229U;
        static const AkUniqueID PLAY_PLAYER_JUMP_VOX = 1177496478U;
        static const AkUniqueID PLAY_PLAYERFOOTSTEP = 1592819191U;
        static const AkUniqueID PLAY_PLAYERJUMP = 3817990921U;
        static const AkUniqueID PLAY_PLAYERLANDING = 2872645466U;
        static const AkUniqueID PLAY_PLAYERSWORDGROW = 93248917U;
        static const AkUniqueID PLAY_SMALLSWORDHIT = 4187859053U;
        static const AkUniqueID PLAY_SMALLSWORDSWING = 1238801504U;
        static const AkUniqueID PLAY_STAGE1_FREEZE = 524979589U;
        static const AkUniqueID PLAY_TREASURE_CHEST = 3939144723U;
        static const AkUniqueID STOP_BIGSWORDDRAG = 2707922347U;
        static const AkUniqueID STOP_COMBAT = 913896232U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace COMBATSTATE
        {
            static const AkUniqueID GROUP = 1071758680U;

            namespace STATE
            {
                static const AkUniqueID BOSSPHASE1 = 851884604U;
                static const AkUniqueID BOSSPHASE2 = 851884607U;
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace COMBATSTATE

        namespace MUSICSTATE
        {
            static const AkUniqueID GROUP = 1021618141U;

            namespace STATE
            {
                static const AkUniqueID BOSS = 1560169506U;
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSICSTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace GAMEPLAYSWITCH
        {
            static const AkUniqueID GROUP = 1516247569U;

            namespace SWITCH
            {
                static const AkUniqueID BOSS = 1560169506U;
                static const AkUniqueID COMBAT = 2764240573U;
            } // namespace SWITCH
        } // namespace GAMEPLAYSWITCH

        namespace PLAYERFOOTSTEP
        {
            static const AkUniqueID GROUP = 3542290436U;

            namespace SWITCH
            {
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID STONE = 1216965916U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace PLAYERFOOTSTEP

        namespace PLAYERLANDING
        {
            static const AkUniqueID GROUP = 214305155U;

            namespace SWITCH
            {
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID STONE = 1216965916U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace PLAYERLANDING

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID REVERB = 348963605U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
