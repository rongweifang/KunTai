using KunTaiServiceLibrary.valueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    [FluorineFx.RemotingService("系统菜单服务")]
    public class Authority
    {
        #region command texts

        private const string role_authority_select_commandText = "SELECT A.ID,A.PID,A.NAME,A.LINK,A.ICON,(CASE WHEN RA.AUTHORITYID IS NULL THEN 0 ELSE 1 END) AS STATE FROM AUTHORITY A LEFT JOIN (SELECT DISTINCT AUTHORITYID FROM ROLEAUTHORITY WHERE ROLEID={0}) RA ON A.ID=RA.AUTHORITYID ORDER BY A.PID,A.SHOWID,A.ID";

        private const string role_authority_delete_commandText = "DELETE ROLEAUTHORITY WHERE ROLEID IN ({0})";

        private const string role_authority_insert_commandText = "INSERT INTO ROLEAUTHORITY (ROLEID,AUTHORITYID) VALUES ({0},'{1}');\n";

        private const string authority_total_commandText = "SELECT COUNT(*) FROM AUTHORITY{0}";


        #endregion


        private enum AuthorityType { MENU = 1, TREE = 2 };

        private string CreateTreeXml(AuthorityObject node)
        {
            List<AuthorityObject> list = new List<AuthorityObject>();
            list.Add(node);
            return CreateTreeXmlByLevel(list, 0);
        }

        private string CreateTreeXmlByLevel(List<AuthorityObject> list, int level)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder _xml = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                AuthorityObject obj = list[i];
                _xml.Append(CreateNodeByLevel(obj, level));
                if (obj.CHILDREN != null && obj.CHILDREN.Count > 0)
                {
                    level++;
                    _xml.Append(CreateTreeXmlByLevel(obj.CHILDREN, level));
                    // sb.Append("</DATA>");
                    level--;

                    // 拼接结束标签
                    if (level == 0)
                        _xml.Append("</DATAS>");
                    else
                        _xml.Append("</DATA>");
                }
            }
            return _xml.ToString();
        }

        private string CreateNodeByLevel(AuthorityObject node, int level)
        {
            StringBuilder _xml = new StringBuilder();
            if (level == 0)
                _xml.Append("<DATAS");
            else
                _xml.Append("<DATA");

            _xml.AppendFormat(" NAME=\"{0}\"", node.NAME);
            _xml.AppendFormat(" ID=\"{0}\"", node.ID);
            _xml.AppendFormat(" PARENTID=\"{0}\"", node.PID);
            _xml.AppendFormat(" LEVEL=\"{0}\"", level);
            _xml.AppendFormat(" ISBRANCH=\"{0}\"", node.CHILDREN != null && node.CHILDREN.Count > 0 ? "TRUE" : "FALSE");
            _xml.AppendFormat(" STATE=\"{0}\"", node.STATE);
            if (node.CHILDREN == null || node.CHILDREN.Count == 0)
            {
                _xml.Append("/>");
            }
            else
            {
                _xml.Append(">");
            }
            return _xml.ToString();
        }

        private void CreateList(Dictionary<string, List<AuthorityObject>> dicAuthority, ref AuthorityObject node)
        {
            if (!dicAuthority.ContainsKey(node.ID.ToString()))
                return;

            node.CHILDREN = dicAuthority[node.ID];
            double count = 0;
            for (int i = 0; i < node.CHILDREN.Count; i++)
            {
                AuthorityObject obj = node.CHILDREN[i];
                CreateList(dicAuthority, ref obj);
                // 如果是全选，加上1
                if (obj.STATE == "1")
                {
                    count += 1;
                }
                // 如果是半选，加上0.5，用来保证父节点的父节点的半选状态
                else if (obj.STATE == "2")
                {
                    count += 0.5;
                }
            }
            if (count == 0)
            {
                // 如果没有子节点被选中，那么当前节点的状态为不选
                node.STATE = "0";
            }
            else if (count >= node.CHILDREN.Count)
            {
                // 如果所有的子节点被选中，那么当前节点的状态为选中
                node.STATE = "1";
            }
            else
            {
                // 如果不是所有的子节点被选中，那么当前节点的状态为半选
                node.STATE = "2";
            }
        }

        private AuthorityObject CreateXmlRoot()
        {
            AuthorityObject authorityObject = new AuthorityObject();
            authorityObject.ID = "359B566F-CE0F-403A-B046-F27752633979";//注意这里的第一级权限的PID要跟这个一致
            authorityObject.PID = "";
            authorityObject.NAME = "所有权限";
            authorityObject.LINK = "";
            authorityObject.ICON = "";
            authorityObject.LEVEL = "0";
            authorityObject.STATE = "0";
            authorityObject.CHILDREN = new List<AuthorityObject>();

            return authorityObject;
        }

        private Dictionary<string, List<AuthorityObject>> InitAuthority(DataSet dataSet, AuthorityType type, bool isShowState)
        {
            Dictionary<string, List<AuthorityObject>> result = new Dictionary<string, List<AuthorityObject>>();

            if (dataSet == null || dataSet.Tables[0].Rows.Count == 0)
                return result;

            string pid = string.Empty;
            List<AuthorityObject> list = new List<AuthorityObject>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                AuthorityObject node = type == AuthorityType.TREE ? CreateObjByDataRow(row, isShowState) : CreateMenuObjByDataRow(row);
                if (!string.IsNullOrEmpty(node.ID) && node.PID != pid)
                {
                    // 当上级权限不同时，重新创建集合
                    list = new List<AuthorityObject>();
                    result.Add(node.PID, list);
                    // 修改上个权限的上级权限ID
                    pid = node.PID;
                }
                // 将上级权限相同的权限放在同一个集合中
                list.Add(node);
            }

            return result;
        }

        private AuthorityObject CreateMenuObjByDataRow(DataRow row)
        {
            AuthorityObject authorityObject = new AuthorityObject();
            authorityObject.ID = row["ID"].ToString().ToUpper();
            authorityObject.PID = row["PID"].ToString().ToUpper();
            authorityObject.NAME = row["NAME"].ToString();
            authorityObject.LINK = row["LINK"].ToString();
            authorityObject.ICON = row["ICON"].ToString();
            authorityObject.STATE = row["STATE"].ToString();

            return authorityObject;
        }

        private AuthorityObject CreateObjByDataRow(DataRow row, bool isShowState)
        {
            AuthorityObject _object = new AuthorityObject();
            _object.ID = row["ID"].ToString().ToUpper();
            _object.PID = row["PID"].ToString().ToUpper();
            _object.NAME = row["NAME"].ToString();

            if (isShowState)
                _object.STATE = row["STATE"].ToString();

            return _object;
        }


        #region get role authority

        public string getRoleAuthority(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string roleID = xml.Element("ID").Value;
            if (roleID.Split(',').Length > 1)
            {
                //指定一个不存在的UUID，用来查询空的权限
                roleID = "'00000000-0000-0000-0000-000000000000'";
            }

            DataSet dataSetRoleAuthority = null;
            try
            {
                dataSetRoleAuthority = new DataAccessHandler().executeDatasetResult(
                    string.Format(role_authority_select_commandText, roleID), null);
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            string result = string.Empty;
            if (dataSetRoleAuthority != null && dataSetRoleAuthority.Tables.Count > 0)
            {
                //获取权限显示的类型
                string type = string.Empty;
                type = xml.Element("TYPE") == null ? AuthorityType.TREE.ToString() : xml.Element("TYPE").Value;

                result = Result.getResultXml(getRoleAuthorityXml(ref dataSetRoleAuthority, type));

                type = null;
            }
            else
            {
                result = Result.getFaultXml(Error.RESULT_ERROR);
            }

            text = null;
            xml = null;
            dataSetRoleAuthority = null;

            return result;
        }

        private string getRoleAuthorityXml(ref DataSet dataSetRoleAuthority, string type)
        {
            AuthorityObject authorityRoot = CreateXmlRoot();

            Dictionary<string, List<AuthorityObject>> dicAuthority = InitAuthority(dataSetRoleAuthority, (type == "MENU" ? AuthorityType.MENU : AuthorityType.TREE), true);
            CreateList(dicAuthority, ref authorityRoot);

            return type == "MENU" ? CreateMenuXml(authorityRoot) : CreateTreeXml(authorityRoot);
        }

        private string CreateMenuXml(AuthorityObject authorityObject)
        {
            StringBuilder menusXml = new StringBuilder("<MENUS>");
            menusXml.Append(CreateMenuXmlByList(authorityObject.CHILDREN));
            menusXml.Append("</MENUS>");
            return menusXml.ToString();
        }

        private string CreateMenuXmlByList(List<AuthorityObject> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder menusXml = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                AuthorityObject obj = list[i];
                menusXml.Append(CreateMenuItem(obj));
                if (obj.CHILDREN != null && obj.CHILDREN.Count > 0)
                {
                    menusXml.Append(CreateMenuXmlByList(obj.CHILDREN));
                    // 拼接结束标签
                    menusXml.Append("</MENUITEM>");
                }
            }
            return menusXml.ToString();
        }

        private string CreateMenuItem(AuthorityObject node)
        {
            StringBuilder menuItem = new StringBuilder("<MENUITEM");
            menuItem.AppendFormat(" LABEL=\"{0}\"", node.NAME);
            menuItem.AppendFormat(" ID=\"{0}\"", node.ID);
            menuItem.AppendFormat(" LINK=\"{0}\"", node.LINK);
            menuItem.AppendFormat(" ICON=\"{0}\"", node.ICON);
            menuItem.AppendFormat(" STATE=\"{0}\"", node.STATE);
            if (node.CHILDREN == null || node.CHILDREN.Count == 0)
            {
                menuItem.Append("/>");
            }
            else
            {
                menuItem.Append(">");
            }
            return menuItem.ToString();
        }

        #endregion



        #region update role authority

        public string updateRoleAuthority(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Result.getFaultXml(Error.XML_IS_NULL);

            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch
            {
                return Result.getFaultXml(Error.XML_FORMAT_ERROR);
            }

            string result = string.Empty;

            try
            {
                string roleID = xml.Element("ID").Value;
                if (!string.IsNullOrEmpty(roleID))
                {
                    //先删除原先已经保存的权限
                    new DataAccessHandler().executeNonQueryResult(
                        string.Format(role_authority_delete_commandText, roleID), null);
                }

                //设置最新的权限
                StringBuilder commandText = new StringBuilder();
                IEnumerable<XElement> dataElements = xml.Element("AUTHORITY").Elements("DATA");
                using (IEnumerator<XElement> enumerator = dataElements.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        commandText.AppendFormat(role_authority_insert_commandText, roleID, enumerator.Current.Attribute("ID").Value);
                    }
                }

                if (!string.IsNullOrEmpty(commandText.ToString()))
                {
                    result = new DataAccessHandler().executeNonQueryResult(commandText.ToString(), null);

                    roleID = null;
                    commandText = null;
                    dataElements = null;
                }
            }
            catch (Exception ex)
            {
                return Result.getFaultXml(ex.Message);
            }

            text = null;
            xml = null;

            return result;
        }

        #endregion

    }
}
