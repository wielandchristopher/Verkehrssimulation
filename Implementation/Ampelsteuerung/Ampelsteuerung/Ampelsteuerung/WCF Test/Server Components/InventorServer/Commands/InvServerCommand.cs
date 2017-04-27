// (C) Copyright 2013 by Autodesk, Inc. 
//
// Written by Philippe Leefsma, December 2013.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Inventor;
using Autodesk.ADN.InvUtility.CommandUtils;

////////////////////////////////////////////////////////////////////////////////////
// InvServerCommand Inventor Add-in Command
//  
// Author: Philippe Leefsma
// Creation date: 12/26/2013 11:24:58 PM
// 
////////////////////////////////////////////////////////////////////////////////////
namespace InventorServer
{
    [AdnCommandAttribute]
    public class InvServerCommand : AdnButtonCommandBase
    {
        public InvServerCommand(ApplicationAddInSite addInSite) :
            base(addInSite.Application)
        {
            AddInSite = addInSite;
        }

        public ApplicationAddInSite AddInSite
        {
            private set;
            get;
        }

        public override string DisplayName
        {
            get
            {
                return "WCF Server";
            }
        }

        public override string InternalName
        {
            get
            {
                return "Autodesk.ADN.InventorServer.InvServerCmd";
            }
        }

        public override CommandTypesEnum Classification
        {
            get
            {
                return CommandTypesEnum.kEditMaskCmdType;
            }
        }

        public override string ClientId
        {
            get
            {
                Type t = typeof(StandardAddInServer);
                return t.GUID.ToString("B");
            }
        }

        public override string Description
        {
            get
            {
                return "ADN WCF Server";
            }
        }

        public override string ToolTipText
        {
            get
            {
                return "ADN WCF Server";
            }
        }

        public override ButtonDisplayEnum ButtonDisplay
        {
            get
            {
                return ButtonDisplayEnum.kDisplayTextInLearningMode;
            }
        }

        public override string StandardIconName
        {
            get
            {
                return "InventorServer.resources.adsk.ico";
            }
        }

        public override string LargeIconName
        {
            get
            {
                return "InventorServer.resources.adsk.ico";
            }
        }

        protected override void OnExecute(NameValueMap context)
        {
            ServerWnd.MakeVisible(
                AddInSite,
                DockingStateEnum.kDockLeft);

            Terminate();
        }

        protected override void OnHelp(NameValueMap context)
        {

        }

        protected override void OnLinearMarkingMenu(
           ObjectsEnumerator SelectedEntities,
           SelectionDeviceEnum SelectionDevice,
           CommandControls LinearMenu,
           NameValueMap AdditionalInfo)
        {
            // Add this button to linear context menu
            //LinearMenu.AddButton(ControlDefinition as ButtonDefinition, true, true, "", false);
        }

        protected override void OnRadialMarkingMenu(
            ObjectsEnumerator SelectedEntities,
            SelectionDeviceEnum SelectionDevice,
            RadialMarkingMenu RadialMenu,
            NameValueMap AdditionalInfo)
        {
            // Add this button to radial context menu
            //RadialMenu.NorthControl = ControlDefinition;
        }
    }
}
