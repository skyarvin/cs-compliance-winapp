using CefSharp;
using CefSharp.WinForms;
using SkydevCSTool.Class;
using System;
using System.Threading.Tasks;
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
                //browser.EvaluateScriptAsync(@"
                //var bounce = $(`body:contains('locked to another bouncer')`).length;
                //    if(bounce > 0){
                //        bound.saveAsBounce();
                //    }
                //");
            }
        }
        public void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (!e.Frame.IsMain)
                return;

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

                    window.onmousedown = function(e) {
                        if(e.which == 2){
                            e.preventDefault();
                            bound.triggerClear();
                        }
                    }

                    window.onkeydown = function(e){
                        if(e.which == 112){
                            e.preventDefault();
                            bound.disableForceHide();
                        }
                    }
                ");

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

            browser.EvaluateScriptAsync(@"
                window.addEventListener('DOMContentLoaded', function(){
                    bound.evaluateMaxRoomDuration();
                });
            ");


            if (Globals.IsBuddySystem())
            {

                browser.ExecuteScriptAsync(@"
                        console.log(`start`);
                        window.addEventListener(`DOMContentLoaded`, function(){
                                document.getElementById(`approve_button`).style.display = `none`;
                        });
                    ");

            }

            Globals.ForceHideComliance = true;
            if (Globals.IsServer())
            {
                Globals.ApprovedAgents.Clear();
                AsynchronousSocketListener.SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count });
                Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count);
            }
        }

        public void DisableForceHide()
        {
            Globals.ForceHideComliance = false;
            browser.EvaluateScriptAsync("$(`#compliance_details,#id_photos`).show()");
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
            try
            {
                Logger log = new Logger { id = Globals.LAST_SUCCESS_ID, action = "BN" };
                if (log.id != 0)
                    log.Update();
            }
            catch (AggregateException e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                Globals.showMessage(String.Concat("Error connecting to Chaturbate servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"));
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                Globals.showMessage(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."));

            }


        }
        public void OnClicked(string id)
        {
            Globals.SaveToLogFile(String.Concat("Bound OnClicked: ", id), (int)LogType.Activity);
            if (HtmlItemClicked != null)
            {
                HtmlItemClicked(this, new HtmlItemClickedEventArgs() { Id = id });
            }
        }

        public void EvaluateMaxRoomDuration()
        {
            Task.Factory.StartNew(() =>
            {

                try
                {
                    Globals.LastRoomChatlog = Logger.GetLastChatlog(Globals.CurrentUrl);
                    //var scrpt = "var marked_index=0;" +
                    //    "$('#chatlog_user .chatlog td.chatlog_date, #data .chatlog td.chatlog_date').each(function(){" +
                    //        "if($.trim(this.innerText) == \"" + Globals.LastRoomChatlog + "\"){" +
                    //        "marked_index = $(this).parent().index() + 1;" +
                    //        "$(this)[0].parentElement.style.background=\"#da1b1b\";" +
                    //        "return false;" +
                    //    "}" +
                    //    "});" +
                    //    "bound.updateMaxRoomDuration(marked_index);";

                    var scrpt = "var marked_index=0;" +
                        "highlight(document.querySelector('#data .chatlog tbody'));" +
                        "highlight(document.querySelector('#chatlog_user .chatlog tbody'));" +
                        "function highlight(selector){" +
                            "if(selector == null){ return; }" +
                            "selector.childNodes.forEach(function(el){" +
                                "el.childNodes.forEach(function(td){" +
                                    "if(td.className == 'chatlog_date'){" +
                                        //"if (td.innerText.indexOf(\"" + Globals.LastRoomChatlog + "\") >= 0){" +
                                        "if (td.innerText.indexOf(\"Oct. 4, 2019, 11:35 p.m.\") >= 0){" +
                                            "td.parentNode.style.background = \"#da1b1b\";" +
                                            "marked_index = Array.prototype.slice.call(el.parentElement.children).indexOf(el) + 1;" +
                                            "return false;" +
                                        "}" +
                                    "}" +
                                "})" +
                            "})" +
                        "};" +
                        "bound.updateMaxRoomDuration(marked_index);";
                    browser.EvaluateScriptAsync(scrpt);
                }
                catch { }
            });
            
        }

        
        public void UpdateMaxRoomDuration(int chatlog_position = 0)
        {

            if (!Globals.IsServer())
                return;

            if (chatlog_position == 0 || chatlog_position > 100)
            {
                Globals.frmMain.max_room_duration = 48;
                AsynchronousSocketListener.SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = "48" });
                return;
            }

            if (chatlog_position > 50)
            {
                Globals.frmMain.max_room_duration += 5;
                int duration = Globals.frmMain.max_room_duration;
                AsynchronousSocketListener.SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = duration.ToString() });
            }
                
        }

        public void TriggerClear()
        {
            Globals.frmMain.BroadCastClearEvent();
        }

        public class HtmlItemClickedEventArgs : EventArgs
        {
            public string Id { get; set; }
        }
    }
}
