﻿<%@ Page Title="" Language="C#" MasterPageFile="~/appaspx/service/ServiceCommon.master" AutoEventWireup="true" CodeBehind="ServiceBlockId.aspx.cs" Inherits="WebManager.appaspx.service.ServiceBlockId" %>
<asp:Content ID="Content1" ContentPlaceHolderID="serviceHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="service_common" runat="server">
    <h2>停封玩家ID</h2>
    ID:&nbsp;&nbsp;<asp:TextBox ID="m_playerId" runat="server" style="width:220px;height:25px"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="onBlockPlayerId" Text="确定" style="width:133px;height:25px"/>
    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    <h2>已停封玩家ID列表</h2>
    <asp:Table ID="m_result" runat="server">
    </asp:Table>
    <asp:Button ID="Button2" runat="server" onclick="onUnBlockPlayerId" Text="解封" style="width:133px;height:25px"/>
</asp:Content>
