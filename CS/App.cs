#region Header
//
// Copyright 2003-2021 by Autodesk, Inc. 
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
//
#endregion // Header

using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Snoop;
using RevitLookup.Snoop.Collectors;

namespace RevitLookup
{
  public class App : IExternalApplication
  {
    static AddInId m_appId = new AddInId( new Guid(
      "356CDA5A-E6C5-4c2f-A9EF-B3222116B8C8" ) );

    // get the absolute path of this assembly
    static string ExecutingAssemblyPath = System.Reflection.Assembly
      .GetExecutingAssembly().Location;

    private AppDocEvents m_appDocEvents;

    public Result OnStartup(
      UIControlledApplication application )
    {
      ModelessWindowHandle.RevitMainWindowHandle = application.MainWindowHandle;
      ExternalExecutor.CreateExternalEvent();
      AddMenu( application );
      AddAppDocEvents( application.ControlledApplication );

      return Result.Succeeded;
    }

    public Result OnShutdown(
      UIControlledApplication application )
    {
      RemoveAppDocEvents();

      return Result.Succeeded;
    }

    private void AddMenu( UIControlledApplication app )
    {
      RibbonPanel rvtRibbonPanel = app.CreateRibbonPanel( "Revit Lookup" );
      PulldownButtonData data = new PulldownButtonData( "Options", "Revit Lookup" );

      RibbonItem item = rvtRibbonPanel.AddItem( data );
      PulldownButton optionsBtn = item as PulldownButton;

      // Add Icons to main RevitLookup Menu
      optionsBtn.Image = GetEmbeddedImage( "RevitLookup.Resources.RLookup-16.png" );
      optionsBtn.LargeImage = GetEmbeddedImage( "RevitLookup.Resources.RLookup-32.png" );
      optionsBtn.AddPushButton(new PushButtonData("HelloWorld", "Hello World...", ExecutingAssemblyPath, typeof(HelloWorld).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Db..", "Snoop DB...", ExecutingAssemblyPath, typeof(CmdSnoopDb).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Current Selection...", "Snoop Current Selection...", ExecutingAssemblyPath, typeof(CmdSnoopModScope).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Pick Face...", "Snoop Pick Face...", ExecutingAssemblyPath, typeof(CmdSnoopModScopePickSurface).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Pick Edge...", "Snoop Pick Edge...", ExecutingAssemblyPath, typeof(CmdSnoopModScopePickEdge).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Pick Linked Element...", "Snoop Linked Element...", ExecutingAssemblyPath, typeof(CmdSnoopModScopeLinkedElement).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Dependent Elements...", "Snoop Dependent Elements...", ExecutingAssemblyPath, typeof(CmdSnoopModScopeDependents).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Active View...", "Snoop Active View...", ExecutingAssemblyPath, typeof(CmdSnoopActiveView).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Snoop Application...", "Snoop Application...", ExecutingAssemblyPath, typeof(CmdSnoopApp).FullName));
      optionsBtn.AddPushButton(new PushButtonData("Search and Snoop...", "Search and Snoop...", ExecutingAssemblyPath, typeof(CmdSearchBy).FullName));
    }

    private void AddAppDocEvents( ControlledApplication app )
    {
      m_appDocEvents = new AppDocEvents( app );
      m_appDocEvents.EnableEvents();
    }

    private void RemoveAppDocEvents()
    {
      m_appDocEvents.DisableEvents();
    }

    static BitmapSource GetEmbeddedImage( string name )
    {
      try
      {
        Assembly a = Assembly.GetExecutingAssembly();
        Stream s = a.GetManifestResourceStream( name );
        return BitmapFrame.Create( s );
      }
      catch
      {
        return null;
      }
    }
  }
}
