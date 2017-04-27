using System;
using Inventor;
using System.Runtime.InteropServices;

////////////////////////////////////////////////////////////////////////////////////
// InventorServer Inventor Add-in
//  
// Author: Administrator
// Creation date: 12/26/2013 11:22:35 PM
// 
////////////////////////////////////////////////////////////////////////////////////
namespace InventorServer
{
    [GuidAttribute("ddbc13f4-c0c0-4b1d-8e18-1d2bc5225145"), ComVisible(true)]
    public class StandardAddInServer : Autodesk.ADN.InvUtility.AddIn.AdnAddInServer
    {
        public override void Activate(
            ApplicationAddInSite addInSiteObject,
            bool firstTime)
        {
            base.Activate(addInSiteObject, firstTime);

            // Forces exported types loading - issue in x64 Release
            System.Reflection.Assembly.GetExecutingAssembly().GetExportedTypes();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override string RibbonResource
        {
            get
            {
                return "InventorServer.resources.ribbons.xml";
            }
        }
    }
}
