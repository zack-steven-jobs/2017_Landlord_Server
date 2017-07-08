#pragma once
#include <net\packet_manager.h>
#include <game_landlord_rank_protocol.pb.h>
#include <net\peer_tcp.h>

class i_game_player;
using namespace game_landlord_rank_protocol;

void init_proc_landlord_protocol();

//�й�
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_Hang, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_Hang_result);

//����
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_ChangeTable, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_ChangeTable_result);

//ϵͳ����
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_SystemTalk, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_notice_SystemTalk);

//����ս��
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_enter_room, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_enter_room_result);

PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_start_match, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_start_match_result);

//�뿪ս��
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_leave_room, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_leave_room_result);

//�������� ������Ϣ
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_get_room_scene_info, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_get_room_scene_info_result);

//�����Ϸ״̬		//����Ϸ֮���ж��Ƿ��ڶ�����
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_check_state, i_game_player);
PACKET_REGEDIT_SEND(packetl2c_check_state_result);

//�������
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_playhand, i_game_player);

//������
PACKET_REGEDIT_RECVGATE_LOG(peer_tcp, packetc2l_rob_landlord, i_game_player);

//֪ͨ��ʼ��Ϸ
PACKET_REGEDIT_SEND(packetl2c_notice_startgame);
//֪ͨ ĳĳ�� ������Ϣ
PACKET_REGEDIT_SEND(packetl2c_notice_who_playhand);
//֪ͨ �����˳�����Ϣ
PACKET_REGEDIT_SEND(packetl2c_notice_playhand);
//֪ͨ �Ƿ���Ҫ������
PACKET_REGEDIT_SEND(packetl2c_notice_rob_landlord);
//֪ͨ ʤ��
PACKET_REGEDIT_SEND(packetl2c_notice_winlose);

PACKET_REGEDIT_SEND(packetl2c_notice_rob_landlord_result);
