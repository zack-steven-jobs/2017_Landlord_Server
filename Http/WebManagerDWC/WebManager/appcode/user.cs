using System.Web.Configuration;
using System.Collections.Generic;

// ��½GM�û����
public class GMUser : SysMgr
{
    // ��½�ʺ���
    public string m_user = "";
    // �ʺ�Ȩ��
    public string m_right = "";
    // ��½����
    public string m_pwd = "";
    public string m_type = "";

    // ��½���ʺ�IP
    public string m_ip = "";
    // Ҫ���������ݿ�IP
    public string m_dbIP="";
    // GM�Ϲҽӵ�DB��ÿ��GM���Բ�����db��ͬ
    private int m_dbId = 0;
    // ���ò������
    private string m_opResult = "";
    // ֮ǰ��URL
    private string m_preURL = "";
    private long m_totalRecord = 0;

    // �Ƿ��л��˷�����
    private bool m_isSwitchDbServer = false;

    public string m_viewChannel = "";

    public string preURL
    {
        get { return m_preURL; }
        set
        {
            if (value != "/appaspx/SelectMachine.aspx")
            {
                m_preURL = value;
            }
        }
    }

    // �û�������ܼ�¼����
    public long totalRecord
    {
        get { return m_totalRecord; }
        set { m_totalRecord = value; }
    }

    public bool isSwitchDbServer
    {
        get { return m_isSwitchDbServer; }
    }

    public GMUser() { }
    public GMUser(AccountInfo info)
    {
        m_user = info.m_user;
        m_right = info.m_right;
        m_pwd = info.m_pwd;
        m_ip = info.m_ip;
        m_type = info.m_type;
        m_viewChannel = info.m_viewChannel;
    }

    // ���عҽӵĲ���DB
    public int getDbServerID()
    {
        return m_dbId;
    }

    // ��ʼ��
    public void init()
    {
        m_dbIP = WebConfigurationManager.AppSettings["account"];
        m_dbId = DBMgr.getInstance().getDbId(m_dbIP);
        m_isSwitchDbServer = false;

        addSys(new QueryMgr());
        addSys(new DyOpMgr());
        addSys(new StatMgr());
        addSys(new ExportMgr());
        initSys();
    }

    // ����Ҫ��������Ϸ���ݿ�
    public bool changeGameDb(string pools)
    {
        int id = DBMgr.getInstance().getDbId(pools);
        if (id == -1)
        {
            return false;
        }
        m_dbId = id;
        m_dbIP = pools;

        m_isSwitchDbServer = true;
        return true;
    }

    public string getOpResultString()
    {
        return m_opResult;
    }

    public void setOpResult(OpRes res)
    {
        m_opResult = OpResMgr.getInstance().getResultString(res);
    }

    // ��ѯ
    public OpRes doQuery(object param, QueryType queryType)
    {
        QueryMgr mgr = getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.doQuery(param, queryType, this);
        return res;
    }

    // ���ز�ѯ���
    public object getQueryResult(QueryType queryType)
    {
        QueryMgr mgr = getSys<QueryMgr>(SysType.sysTypeQuery);
        return mgr.getQueryResult(queryType);
    }

    // ���ز�ѯ���
    public object getQueryResult(object param, QueryType queryType)
    {
        QueryMgr mgr = getSys<QueryMgr>(SysType.sysTypeQuery);
        return mgr.getQueryResult(param, queryType, this);
    }

    public OpRes doStat(object param, StatType statName)
    {
        StatMgr mgr = getSys<StatMgr>(SysType.sysTypeStat);
        return mgr.doStat(param, statName, this);
    }

    public object getStatResult(StatType statName)
    {
        StatMgr mgr = getSys<StatMgr>(SysType.sysTypeStat);
        return mgr.getStatResult(statName);
    }

    public OpRes doDyop(object param, DyOpType type)
    {
        DyOpMgr mgr = getSys<DyOpMgr>(SysType.sysTypeDyOp);
        return mgr.doDyop(param, type, this);
    }

    public bool isAdmin() { return m_type == "admin"; }
}

