using monacos.us.web.api.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace monacos.us.web.api.Services
{
    public class NavigationService : INavigation
    {

        readonly string _dBConnectionString;

        public NavigationService(string dBConnectionString) {

            this._dBConnectionString = dBConnectionString;

        }

        public List<MenuItemDTO> BuildNavigationMenu(int NavigationTypeID, int User_ID)
        {

            List<MenuItemDTO> objRootMenuItems;

            try
            {

                objRootMenuItems = BuildNavigationMenu_RootNodes(NavigationTypeID, User_ID);

            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService::BuildNavigationMenu Error: " + objException.Message);
            }

            finally
            {

            }

            return objRootMenuItems;

        }


        public List<TreeViewItemDTO> BuildTreeView(int NavigationTypeID)
        {

            List<TreeViewItemDTO> objRootMenuItems;

            try
            {

                objRootMenuItems = BuildTreeView_RootNodes(NavigationTypeID);

            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService::BuildNavigationMenu Error: " + objException.Message);
            }

            finally
            {

            }

            return objRootMenuItems;


        }




        // TreeView Builder
        protected List<TreeViewItemDTO> BuildTreeView_RootNodes(int Navigation_TypeID)
        {

            SqlDataAdapter objSqlDataAdapter = null;
            SqlCommand objSqCommand = null;
            SqlConnection objSqlConnection = null;
            int level = 1;


            string strSQL = "";

            System.Data.DataTable objDataTable_RootNodes = null;
            System.Data.DataTable objDataTable_ChildNodes = null;
            TreeViewItemDTO objTreeViewItem = null;
            
           
            List<TreeViewItemDTO> rootLevelTreeViewList = new List<TreeViewItemDTO>();
            


            try
            {

                // This Query Also Has Some Additional Logic To Remove Navigation Items Better Rendered with one Root Item in a Tree View
                // But Rendered with out that Root Item in a Menu View
                strSQL = "SELECT Navigation_Items.Navigation_Item_Caption, Navigation_Items.Navigation_Item_URI, Navigation_Items.Navigation_Item_Level_Sort_Order, " +
                         " Navigation_Items.Parent_Navigation_Item_ID,  " +
                         " Navigation_Items.Navigation_Item_ID, Navigation_Types.Navigation_Type_ID, Navigation_Items.Navigation_Item_URI_Target,  Navigation_Items.Navigation_Item_Image " +
                         " FROM Navigation_Types INNER JOIN " +
                         " Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID " +
                         " WHERE (Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID) AND " +
                         "  ( Navigation_Items.Navigation_Item_TreeView_Visible = 1 ) AND " +
                         " (  " +
                         " (Navigation_Items.Parent_Navigation_Item_ID IS NULL AND Navigation_Items.Navigation_Item_TreeView_Visible = 1 ) " +
                         " OR ( " +
                             "  Navigation_Items.Parent_Navigation_Item_ID IN ( " +
                             "  SELECT Navigation_Items .Navigation_Item_ID " +
                             "  FROM Navigation_Types INNER JOIN " +
                             "  Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID " + 
                             "  WHERE Navigation_Items.Parent_Navigation_Item_ID IS NULL AND Navigation_Items.Navigation_Item_TreeView_Visible = 0 AND  Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID " +
                            " ) " +
                         " )" +
                        ") ORDER BY Navigation_Items.Navigation_Item_Level_Sort_Order ";

                try
                {

                    objSqlConnection = new SqlConnection(this._dBConnectionString);
                }

                catch (System.Exception objException)
                {
                    throw new Exception("Failure to connect to database. Error: " + objException.Message);
                }


                objSqCommand = new SqlCommand(strSQL, objSqlConnection);
                objSqCommand.Parameters.AddWithValue("@Navigation_Type_ID", Navigation_TypeID);
               

                objSqlDataAdapter = new SqlDataAdapter(objSqCommand);

                objDataTable_RootNodes = new DataTable();

                objSqlDataAdapter.Fill(objDataTable_RootNodes);

                // Now Get All Child Nodes (Nested N Levels Deep) in One Data Table
                // For Given Navigation Type ID

                strSQL = "SELECT Navigation_Items.Navigation_Item_Caption, Navigation_Items.Navigation_Item_URI, Navigation_Items.Navigation_Item_Level_Sort_Order, " +
                         " Navigation_Items.Parent_Navigation_Item_ID, " +
                         " Navigation_Items.Navigation_Item_ID, Navigation_Types.Navigation_Type_ID, Navigation_Items.Navigation_Item_URI_Target, Navigation_Items.Navigation_Item_Image " +
                         " FROM Navigation_Types INNER JOIN " +
                         " Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID " +
                         " WHERE (Navigation_Items.Parent_Navigation_Item_ID IS NOT NULL) AND " +
                         " (Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID) AND " +
                         " (Navigation_Items.Navigation_Item_TreeView_Visible = 1 )";


                objSqCommand = new SqlCommand(strSQL, objSqlConnection);
                objSqCommand.Parameters.AddWithValue("@Navigation_Type_ID", Navigation_TypeID);
               

                objSqlDataAdapter = new SqlDataAdapter(objSqCommand);

                objDataTable_ChildNodes = new DataTable();

                objSqlDataAdapter.Fill(objDataTable_ChildNodes);




                // Start With Root Nodes 
                foreach (System.Data.DataRow objDataRow_Node in objDataTable_RootNodes.Rows)
                {

                    // Render Node

                    objTreeViewItem = new TreeViewItemDTO();
                    objTreeViewItem.children = new List<TreeViewItemDTO>();


                    if (objDataRow_Node["Navigation_Item_Caption"] != System.DBNull.Value)
                    {
                        objTreeViewItem.text = objDataRow_Node["Navigation_Item_Caption"].ToString().Trim();


                    }


                    if (objDataRow_Node["Navigation_Item_URI"] != System.DBNull.Value)
                    {

                        if (objDataRow_Node["Navigation_Item_URI"].ToString().IndexOf("http", 0) != -1)
                        {

                            // External URL Leaves As It Is
                            objTreeViewItem.url = objDataRow_Node["Navigation_Item_URI"].ToString().Trim();

                        }
                        else
                        {


                            // Web App MVC Route URI 
                            objTreeViewItem.url = objDataRow_Node["Navigation_Item_URI"].ToString().Trim();  



                        }


                    }


                    if (objDataRow_Node["Navigation_Item_URI_Target"] != System.DBNull.Value)
                    {
                        
                        objTreeViewItem.target = objDataRow_Node["Navigation_Item_URI_Target"].ToString().Trim();
                    }



                    if (objDataRow_Node["Navigation_Item_Image"] != System.DBNull.Value)
                    {
                        objTreeViewItem.imageUrl = objDataRow_Node["Navigation_Item_Image"].ToString().Trim();

                    }


                    objTreeViewItem.level = level;



                    // Navigation Item ID
                    objTreeViewItem.NavigationItemID = objDataRow_Node["Navigation_Item_ID"].ToString().Trim();

                    //RootLevelMenu.items.Add(objTreeViewItem);
                    rootLevelTreeViewList.Add(objTreeViewItem);


                    // Render Children If Neccessary

                    BuildTreeView_ChildNodes(ref objTreeViewItem, ref objDataTable_ChildNodes, level);

                }
              

            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService::BuildTreeView_RootNodes Error: " + objException.Message);
            }

            finally
            {

                if (objSqlConnection != null && objSqlConnection.State != System.Data.ConnectionState.Closed) {

                    objSqlConnection.Close();

                }


                objSqlDataAdapter = null;
                objDataTable_RootNodes = null;
                objSqCommand = null;

            }
         
            return rootLevelTreeViewList;


        }


       // Menu Builder
        protected List<MenuItemDTO> BuildNavigationMenu_RootNodes(int Navigation_TypeID, int User_ID)
        {

            SqlDataAdapter objSqlDataAdapter = null;
            SqlCommand objSqCommand = null;
            SqlConnection objSqlConnection = null;
            int level = 1;
            string strSQL = "";

            System.Data.DataTable objDataTable_RootNodes = null;
            System.Data.DataTable objDataTable_ChildNodes = null;
            MenuItemDTO objMenuItem = null;

            //MenuItemDTO RootLevelMenu = new MenuItemDTO();

            List<MenuItemDTO> RootLevelMenuList = new List<MenuItemDTO>();

           



            try
            {

                // This Query Also Has Some Additional Logic To Remove Navigation Items Better Rendered with one Root Item in a Tree View
                // But Rendered with out that Root Item in a Menu View
                strSQL = "SELECT Navigation_Items.Navigation_Item_Caption, Navigation_Items.Navigation_Item_URI, Navigation_Items.Navigation_Item_Level_Sort_Order, " +
                         " Navigation_Items.Parent_Navigation_Item_ID, NavigationItem_ResourceRole_Xref.Resource_Role_ID,  " +
                         " Navigation_Items.Navigation_Item_ID, Navigation_Types.Navigation_Type_ID, Navigation_Items.Navigation_Item_URI_Target,  Navigation_Items.Navigation_Item_Image " +
                         " FROM Navigation_Types INNER JOIN " +
                         " Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID INNER JOIN " +
                         " NavigationItem_ResourceRole_Xref ON Navigation_Items.Navigation_Item_ID = NavigationItem_ResourceRole_Xref.Navigation_Item_ID " +
                         " INNER JOIN User_Role_Xref ON User_Role_Xref.Resource_Role_ID = NavigationItem_ResourceRole_Xref.Resource_Role_ID " +
                         " WHERE  (User_Role_Xref.User_ID = @User_ID) AND " +
                         " (Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID) AND " +
                         "  ( Navigation_Items.Navigation_Item_Menu_Visible = 1 ) AND " +
                         " (  " +
                         " (Navigation_Items.Parent_Navigation_Item_ID IS NULL AND Navigation_Items.Navigation_Item_Menu_Visible = 1 ) " +
                         " OR ( " +
                             "  Navigation_Items.Parent_Navigation_Item_ID IN ( " +
                             "  SELECT Navigation_Items .Navigation_Item_ID " +
                             "  FROM Navigation_Types INNER JOIN " +
                             "  Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID INNER JOIN " +
                             "  NavigationItem_ResourceRole_Xref ON Navigation_Items.Navigation_Item_ID = NavigationItem_ResourceRole_Xref.Navigation_Item_ID " +
                             "  INNER JOIN  User_Role_Xref ON User_Role_Xref.Resource_Role_ID = NavigationItem_ResourceRole_Xref.Resource_Role_ID " +
                             "  WHERE  User_Role_Xref.User_ID = @User_ID  AND Navigation_Items.Parent_Navigation_Item_ID IS NULL AND Navigation_Items.Navigation_Item_Menu_Visible = 0  AND  Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID " +
                            " ) " +
                         " )" +
                        ") ORDER BY Navigation_Items.Navigation_Item_Level_Sort_Order ";

                try
                {

                    objSqlConnection = new SqlConnection(this._dBConnectionString);
                }

                catch (System.Exception objException)
                {
                    throw new Exception("Failure to connect to database. Error: " + objException.Message);
                }


                objSqCommand = new SqlCommand(strSQL, objSqlConnection);
                objSqCommand.Parameters.AddWithValue("@Navigation_Type_ID", Navigation_TypeID);
                objSqCommand.Parameters.AddWithValue("@User_ID", User_ID);   

                objSqlDataAdapter = new SqlDataAdapter(objSqCommand);

                objDataTable_RootNodes = new DataTable();

                objSqlDataAdapter.Fill(objDataTable_RootNodes);

                // Now Get All Child Nodes (Nested N Levels Deep) in One Data Table
                // For Given Navigation Type ID and Role

                strSQL = "SELECT Navigation_Items.Navigation_Item_Caption, Navigation_Items.Navigation_Item_URI, Navigation_Items.Navigation_Item_Level_Sort_Order, " +
                         " Navigation_Items.Parent_Navigation_Item_ID, NavigationItem_ResourceRole_Xref.Resource_Role_ID,  " +
                         " Navigation_Items.Navigation_Item_ID, Navigation_Types.Navigation_Type_ID, Navigation_Items.Navigation_Item_URI_Target, Navigation_Items.Navigation_Item_Image " +
                         " FROM Navigation_Types INNER JOIN " +
                         " Navigation_Items ON Navigation_Types.Navigation_Type_ID = Navigation_Items.Navigation_Type_ID INNER JOIN " +
                         " NavigationItem_ResourceRole_Xref ON Navigation_Items.Navigation_Item_ID = NavigationItem_ResourceRole_Xref.Navigation_Item_ID " +
                         " INNER JOIN  User_Role_Xref ON User_Role_Xref.Resource_Role_ID = NavigationItem_ResourceRole_Xref.Resource_Role_ID " +
                         " WHERE  (User_Role_Xref.User_ID = @User_ID) AND (Navigation_Items.Parent_Navigation_Item_ID IS NOT NULL) AND " +
                         " (Navigation_Types.Navigation_Type_ID = @Navigation_Type_ID) AND " +
                         " (Navigation_Items.Navigation_Item_Menu_Visible = 1 )";


                objSqCommand = new SqlCommand(strSQL, objSqlConnection);
                objSqCommand.Parameters.AddWithValue("@Navigation_Type_ID", Navigation_TypeID);
                objSqCommand.Parameters.AddWithValue("@User_ID", User_ID );  // OCM This Parameter Must Change

                objSqlDataAdapter = new SqlDataAdapter(objSqCommand);

                objDataTable_ChildNodes = new DataTable();

                objSqlDataAdapter.Fill(objDataTable_ChildNodes);




                // Start With Root Nodes 
                foreach (System.Data.DataRow objDataRow_Node in objDataTable_RootNodes.Rows)
                {

                    // Render Node

                    objMenuItem = new MenuItemDTO();
                    objMenuItem.children = new List<MenuItemDTO>();


                    if (objDataRow_Node["Navigation_Item_Caption"] != System.DBNull.Value)
                    {
                        objMenuItem.text = objDataRow_Node["Navigation_Item_Caption"].ToString().Trim();


                    }


                    if (objDataRow_Node["Navigation_Item_URI"] != System.DBNull.Value)
                    {

                        if (objDataRow_Node["Navigation_Item_URI"].ToString().IndexOf("http", 0) != -1)
                        {

                            // External URL Leaves As It Is
                            objMenuItem.url = objDataRow_Node["Navigation_Item_URI"].ToString().Trim();

                        }
                        else
                        {


                            // Web App MVC Route URI 
                            objMenuItem.url = objDataRow_Node["Navigation_Item_URI"].ToString().Trim();



                        }


                    }


                    if (objDataRow_Node["Navigation_Item_URI_Target"] != System.DBNull.Value)
                    {

                        objMenuItem.target = objDataRow_Node["Navigation_Item_URI_Target"].ToString().Trim();
                    }



                    if (objDataRow_Node["Navigation_Item_Image"] != System.DBNull.Value)
                    {
                        objMenuItem.imageUrl = objDataRow_Node["Navigation_Item_Image"].ToString().Trim();

                    }

                    // Menu Level Depth
                    objMenuItem.level = level;

                    // Navigation Item ID
                    objMenuItem.NavigationItemID = objDataRow_Node["Navigation_Item_ID"].ToString().Trim();

                    
                    RootLevelMenuList.Add(objMenuItem);


                    // Render Children If Neccessary

                    BuildNavigationMenu_ChildNodes(ref objMenuItem, ref objDataTable_ChildNodes, level );

                }


            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService::BuildNavigationMenu_RootNodes Error: " + objException.Message);
            }

            finally
            {

                if (objSqlConnection != null && objSqlConnection.State != System.Data.ConnectionState.Closed)
                {

                    objSqlConnection.Close();

                }


                objSqlDataAdapter = null;
                objDataTable_RootNodes = null;
                objSqCommand = null;

            }

            return RootLevelMenuList;


        }



        protected void BuildTreeView_ChildNodes(ref TreeViewItemDTO objParentTreeViewItem, ref System.Data.DataTable objChildNodeDataTable, int level)
        {

            
            System.Data.DataView? objDataView = null;
            TreeViewItemDTO? objTreeViewItem = null;



            try
            {

                // Menu Level Depth Increment

                level = level + 1;


                objDataView = new DataView(objChildNodeDataTable);


                // Filter All Children Of Parent ID Passed In Via
                // Tree Node Value
                objDataView.RowFilter = "Parent_Navigation_Item_ID = " + Convert.ToInt32(objParentTreeViewItem.NavigationItemID);
                objDataView.Sort = "Navigation_Item_Level_Sort_Order ASC";


                foreach (System.Data.DataRowView objDataViewRow_Node in objDataView)
                {


                    // Render Node

                    objTreeViewItem = new TreeViewItemDTO();
                    objTreeViewItem.children = new List<TreeViewItemDTO>();

                    if (objDataViewRow_Node["Navigation_Item_Caption"] != System.DBNull.Value)
                    {
                        objTreeViewItem.text = objDataViewRow_Node["Navigation_Item_Caption"].ToString().Trim();

                    }


                    if (objDataViewRow_Node["Navigation_Item_URI"].ToString().IndexOf("http", 0) != -1)
                    {

                        // External URL Leaves As It Is
                        objTreeViewItem.url = objDataViewRow_Node["Navigation_Item_URI"].ToString().Trim();

                    }
                    else
                    {


                        // Web App MVC Route URI 
                        objTreeViewItem.url = objDataViewRow_Node["Navigation_Item_URI"].ToString().Trim();


                    }

                    if (objDataViewRow_Node["Navigation_Item_URI_Target"] != System.DBNull.Value)
                    {


                        objTreeViewItem.target = objDataViewRow_Node["Navigation_Item_URI_Target"].ToString().Trim();

                    }


                    if (objDataViewRow_Node["Navigation_Item_Image"] != System.DBNull.Value)
                    {
                        objTreeViewItem.imageUrl = objDataViewRow_Node["Navigation_Item_Image"].ToString().Trim();

                    }


                    // Tree Level Depth
                    objTreeViewItem.level = level;

                    // Navigation Item ID
                    objTreeViewItem.NavigationItemID = objDataViewRow_Node["Navigation_Item_ID"].ToString().Trim();


                    objParentTreeViewItem.children.Add(objTreeViewItem);


                    // Render Children If Neccessary

                    BuildTreeView_ChildNodes(ref objTreeViewItem, ref objChildNodeDataTable, level);


                }

            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService:BuildTreeView_ChildNodes Error: " + objException.Message);
            }

            finally
            {
                objDataView = null;
            }


        }





        protected void BuildNavigationMenu_ChildNodes(ref MenuItemDTO objParentMenuItem, ref System.Data.DataTable objChildNodeDataTable, int level)
        {

            System.Data.DataView? objDataView = null;
            MenuItemDTO? objMenuItem = null;



            try
            {

                // Menu Level Depth Increment

                level = level + 1;


                objDataView = new DataView(objChildNodeDataTable);


                // Filter All Children Of Parent ID Passed In Via
                // Tree Node Value
                objDataView.RowFilter = "Parent_Navigation_Item_ID = " + Convert.ToInt32(objParentMenuItem.NavigationItemID);
                objDataView.Sort = "Navigation_Item_Level_Sort_Order ASC";


                foreach (System.Data.DataRowView objDataViewRow_Node in objDataView)
                {


                    // Render Node

                    objMenuItem = new MenuItemDTO();
                    objMenuItem.children = new List<MenuItemDTO>();

                    if (objDataViewRow_Node["Navigation_Item_Caption"] != System.DBNull.Value)
                    {
                        objMenuItem.text = objDataViewRow_Node["Navigation_Item_Caption"].ToString().Trim();

                    }


                    if (objDataViewRow_Node["Navigation_Item_URI"].ToString().IndexOf("http", 0) != -1)
                    {

                        // External URL Leaves As It Is
                        objMenuItem.url = objDataViewRow_Node["Navigation_Item_URI"].ToString().Trim();

                    }
                    else
                    {


                        // Web App MVC Route URI 
                        objMenuItem.url = objDataViewRow_Node["Navigation_Item_URI"].ToString().Trim();


                    }

                    if (objDataViewRow_Node["Navigation_Item_URI_Target"] != System.DBNull.Value)
                    {


                        objMenuItem.target = objDataViewRow_Node["Navigation_Item_URI_Target"].ToString().Trim();

                    }


                    if (objDataViewRow_Node["Navigation_Item_Image"] != System.DBNull.Value)
                    {
                        objMenuItem.imageUrl = objDataViewRow_Node["Navigation_Item_Image"].ToString().Trim();

                    }


                    // Menu Level Depth
                    objMenuItem.level = level;

                    // Navigation Item ID
                    objMenuItem.NavigationItemID = objDataViewRow_Node["Navigation_Item_ID"].ToString().Trim();


                    objParentMenuItem.children.Add(objMenuItem);


                    // Render Children If Neccessary

                    BuildNavigationMenu_ChildNodes(ref objMenuItem, ref objChildNodeDataTable, level);


                }

            }

            catch (System.Exception objException)
            {
                throw new Exception("NavigationService:BuildNavigationMenu_ChildNodes Error: " + objException.Message);
            }

            finally
            {
                objDataView = null;
            }


        }
    }
}
