﻿using CefSharp;
using CefSharp.WinForms;
using SkydevCSTool.Class;
using System;
using WindowsFormsApp1;
using WindowsFormsApp1.Models;

namespace SkydevCSTool
{
    public class BoundObject
    {
        public delegate void ItemClickedEventHandler(object sender, HtmlItemClickedEventArgs e);
        public event ItemClickedEventHandler HtmlItemClicked;
        private ChromiumWebBrowser browser;
        public BoundObject(ChromiumWebBrowser br) { browser = br; }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                browser.EvaluateScriptAsync(@"
                    var bounce = $(`body:contains('locked to another bouncer')`).length;
                    if(bounce > 0){
                        bound.saveAsBounce();
                    }
                ");

                browser.ExecuteScriptAsync(@"
                    window.onclick = function(e) { 
                        if (e.target.id != null || e.target.id.length > 0 || e.target.name || e.target.value) { 
                            bound.windowOnClicked(e.target.id + '::[name]='+e.target.name+'::[value]='+e.target.value+'::[url]='+ window.location.href ); 
                        }

                        if (e.target.id != null || e.target.id.length > 0 || e.target.value) {
                            var element_ids = ['approve_button','violation-submit','spammer-submit','request-review-submit','agree_button','disagree_button','reply_button'];
                            if ((e.target.id != undefined) && (element_ids.includes(e.target.id))){
                                console.log(e.target.id);
                                bound.onClicked(e.target.id);
                                return;
                            }

                            var element_values = {
                                  'Update Expiration Date': ['set_expr'],
                                  'Report Identification Missing Problem': ['id-missing'],
                                  'Change Gender': ['change_gender']
                                    }

                            if ((e.target.value != undefined) && (element_values.hasOwnProperty(e.target.value))) {
                                    bound.onClicked(element_values[e.target.value][0]);
                            }
                        }
                    }
                ");
            }
        }
        public void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var submit_script = @"
                    window.onmousemove = function(e) { 
                        bound.onBrowserEvent(); 
                    }
                
                    var zoomLevel = 1;
                    window.addEventListener('wheel', function(e) {
                        if (e.deltaY < 0) {
                             if(event.ctrlKey && zoomLevel < 2){
                                    console.log('scrolling up');
                                    zoomLevel += 0.1;
                                     document.body.style.zoom = zoomLevel;
                             }     
                        }
                        if (e.deltaY > 0) {
                           if(event.ctrlKey && zoomLevel > 0.5){
                                    console.log('scrolling down');
                                    zoomLevel -= 0.1;
                                    document.body.style.zoom = zoomLevel;
                            }
                        }
                    });
                ";
                browser.EvaluateScriptAsync(submit_script);
            }
        }

        public void WindowOnClicked(string element_id)
        {
            Globals.SaveToLogFile(String.Concat("Window OnClicked: ", element_id), (int)LogType.UserClick);
        }
        public void OnBrowserEvent()
        {
            Globals._wentIdle = DateTime.MaxValue;
            Globals._idleTicks = 0;
        }
        public void SaveAsBounce()
        {
            Logger log = new Logger { id = Globals.LAST_SUCCESS_ID, action = "BN" };
            log.Update();
        }
        public void OnClicked(string id)
        {
            Globals.SaveToLogFile(String.Concat("Bound OnClicked: ", id), (int)LogType.Activity);
            if (HtmlItemClicked != null)
            {
                HtmlItemClicked(this, new HtmlItemClickedEventArgs() { Id = id });
            }
        }
        public class HtmlItemClickedEventArgs : EventArgs
        {
            public string Id { get; set; }
        }
    }
}
