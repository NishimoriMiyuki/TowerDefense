public class Singleton<T> where T : class, new()
{
    #region メンバ変数
    /*===============================================================*/
    /**
    * @brief インスタンス変数への代入が完了するまで、アクセスできなくなるジェネリックなインスタンス
    */
    private static volatile T m_instance;
    /**
    * @brief ロックするためのインスタンス
    */
    private static object m_sync_obj = new object();
    /*===============================================================*/
    #endregion

    #region アクセサ変数
    /*===============================================================*/
    /**
    * @brief ジェネリックなインスタンス
    */
    public static T Instance
    {
        get
        {
            // ダブルチェック ロッキング アプローチ.
            if (m_instance == null)
            {
                // m_sync_objインスタンスをロックし、この型そのものをロックしないことで、デッドロックの発生を回避
                lock (m_sync_obj)
                {
                    if (m_instance == null)
                    {
                        m_instance = new T();
                    }
                }
            }
            return m_instance;
        }
    }
    /*===============================================================*/
    #endregion

    /*===============================================================*/
    /**
    * @brief コンストラクタ
    */
    protected Singleton() { }
    /*===============================================================*/
}
/*===============================================================*/