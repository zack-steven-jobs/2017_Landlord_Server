#pragma once

#include <enable_smart_ptr.h>
#include <enable_object_pool.h>
#include <enable_hashmap.h>

#include <game_object.h>
#include <game_object_field.h>
#include <game_object_map.h>
#include <game_object_array.h>

class logic_player;
class logic_lobby;
class logic_table;

typedef boost::shared_ptr<logic_player> LPlayerPtr;
typedef boost::shared_ptr<logic_table> LTablePtr;

typedef std::map<uint32_t, LPlayerPtr> LPLAYER_MAP;
typedef std::map<uint32_t, LTablePtr> LTABLE_MAP;

#define SAFE_DELETE(v) if(v != nullptr){delete v; v = nullptr;}

#ifndef _DEBUG
#define NDEBUG
#endif