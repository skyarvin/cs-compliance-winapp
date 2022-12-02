using CefSharp;
using CefSharp.WinForms;
using CSTool.Class;
using CSTool.Models;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp1;
using WindowsFormsApp1.Models;

namespace CSTool
{
    public class BoundObject
    {
        public delegate void ItemClickedEventHandler(object sender, HtmlItemClickedEventArgs e);
        public event ItemClickedEventHandler HtmlItemClicked;
        private ChromiumWebBrowser browser;
        public BoundObject(ChromiumWebBrowser br) { browser = br; }
        public string sha256Image { get; set; }
        public string prevImage { get; set; }
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            
        }
        public void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Console.WriteLine(e.Url);
            if (!e.Frame.IsMain)
                return;

            Globals.frmMain.hideBanner();
            if (!string.IsNullOrEmpty(Settings.Default.compliance_default_view) && (e.Url.Contains("/compliance/show") || e.Url.Contains("/compliance/photoset")))
            {
                browser.ExecuteScriptAsync(
                "function sleep(ms) {" +
                    "return new Promise(resolve => setTimeout(resolve, ms));" +
                "}" +

                "document.addEventListener('DOMContentLoaded', " +
                    "async function(){" +
                        "await sleep(1000);" +
                        "if(document.getElementById(\"data\") != null){" +
                            "document.getElementById(\"data\").innerText = null;}" +
                        "changeDiv('" + Settings.Default.compliance_default_view + "');});");
            }

            browser.ExecuteScriptAsync(@"
                   window.onclick = function(e) { 
                        if (e.target.id != null || e.target.id.length > 0 || e.target.name || e.target.value) { 
                            bound.windowOnClicked(e.target.id + '::[name]='+e.target.name+'::[value]='+e.target.value+'::[url]='+ window.location.href ); 
                        }

                        if (e.target.id != undefined && e.target.id != null || e.target.id.length > 0 || e.target.value) {
                            var element_ids = ['approve_button','violation-submit','spammer-submit','request-review-submit','agree_button','disagree_button','reply_button','request_photo_button'];

                            if (element_ids.includes(e.target.id)) {
                                console.log(e.target.id);
                                bound.onClicked(e.target.id);
                                return;
                            }

                            var element_values = {
                                  'Update Expiration Date': ['set_expr'],
                                  'Report Identification Missing Problem': ['id-missing'],
                                  'Change Gender': ['change_gender']
                            }

                            if (element_values.hasOwnProperty(e.target.value)) {
                                    bound.onClicked(element_values[e.target.value][0]);
                            }
                            
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
                                    zoomLevel += 0.1;
                                     document.body.style.zoom = zoomLevel;
                             }     
                        }
                        if (e.deltaY > 0) {
                           if(event.ctrlKey && zoomLevel > 0.5){
                                    zoomLevel -= 0.1;
                                    document.body.style.zoom = zoomLevel;
                            }
                        }
                    });

                    function waitUntil(selector, max_ms_timeout){
                        return new Promise( (resolve, reject) => {
	                        var ms = 0;
                            var finder = setInterval( () => {
		                        if (ms >= max_ms_timeout){
			                        clearInterval(finder);
			                        reject(false);
		                        }
                                if (document.querySelectorAll(selector).length > 0){
        	                        clearInterval(finder);
                                    resolve(document.querySelectorAll(selector));
                                }
		                        ms+=100;
                            }, 100);
	
                        });
                    }

                    function highlight(selector, last_chatlog)
                    {
                                
                        var chatlog_position = 0;
                        if (selector == null || selector.length == 0)
                            return; 
                        var found = false
                        var row_index = 0;
                                
                        if (last_chatlog == '' || last_chatlog == undefined || last_chatlog == null){
                            return selector.length;
                        }
                                
                        selector.forEach(function(el)
                        {
                            row_index++;
                                    
                            el.childNodes.forEach(function(td)
                            {
                                if ((td.className == 'chatlog_date') && (td.innerText.indexOf(last_chatlog) >= 0))
                                {
                                    td.parentNode.style.background = '#da1b1b';
                                    chatlog_position = row_index;
                                }
                            })

                        });

                        if (chatlog_position == 0)
                            return row_index 

                        return chatlog_position;
                                
                    }

                    function cacheBuster()
                    {
                        var timestamp = new Date();
                        var img_list = document.querySelectorAll('#id_image');
                        for (let item of img_list) {
                            item.src = item.src+'&'+timestamp.getTime();
                        }
                    }

                    function compareImages()
                    {
                        var ids = document.querySelectorAll('#id_image')[0];
                        if(ids != null)
                        {
                            bound.compareImages(ids.src, window.location.pathname);
                        }
                    }

                    function storeImages()
                    {
                        var ids = document.querySelectorAll('#id_image')[0];
                        if(ids != null)
                        {
                            bound.storeImages(ids.src);
                        }
                    }
                ";

            browser.EvaluateScriptAsync(submit_script);

            browser.EvaluateScriptAsync(@"
                document.addEventListener('DOMContentLoaded', function(){
                    var urlParams = new URLSearchParams(window.location.search);
                    if(urlParams.get('chatstart') != null && urlParams.get('chatend') != null){
                        function qa_chatlog_highlight(){
                            waitUntil('#data .chatlog tbody tr', 5000).then((element) => highlight(element, urlParams.get('chatstart')), (error) => console.log(error));
                            waitUntil('#data .chatlog tbody tr', 5000).then((element) => highlight(element, urlParams.get('chatend')), (error) => console.log(error));
                            waitUntil('#chatlog_user .chatlog tbody tr', 5000).then((element) => highlight(element, urlParams.get('chatstart')), (error) => console.log(error));
                            waitUntil('#chatlog_user .chatlog tbody tr', 5000).then((element) => highlight(element, urlParams.get('chatend')), (error) => console.log(error));
                        }
                        qa_chatlog_highlight();
                        document.getElementById('chatlog_user').addEventListener('DOMSubtreeModified', function()
                        {
                            qa_chatlog_highlight();
                        });
                    } else {
                        bound.evaluateMaxRoomDuration();
                    }
                    
                    
                    window.onkeydown = function(e){
                        if (e.which == 112)
                        {
                            e.preventDefault();
                            bound.disableForceHide();
                        }

                        if(e.ctrlKey == true && e.keyCode == 192){
                            e.preventDefault();
                        }
                    }

                    if (document.getElementsByTagName('body')[0].innerText.indexOf('locked to another bouncer') >= 0)
                    {
                        bound.saveAsBounce();
                    }

                    var followRaw = $('#room_info').children().eq(2).text();
                    bound.showTierLevelBanner(followRaw);

                    //waitUntil(`#id_photos`,5000).then((el) => cacheBuster(), (err) => console.log(`img not found`));
                });
            ");

            if (e.Url.Contains("/compliance/seed_failure") && !string.IsNullOrEmpty(Globals.LastSuccessUrl) && e.Url.Contains(Globals.frmMain.ExtractUsername(Globals.LastSuccessUrl)))
            {
                browser.EvaluateScriptAsync(@"
                    document.addEventListener('DOMContentLoaded', function(){
                        waitUntil(`#id_photos`,5000).then((el) => compareImages(), (err) => console.log(`img for comparing not found`));
                    });
                ");
            }
            else
            {
                browser.EvaluateScriptAsync(@"
                    document.addEventListener('DOMContentLoaded', function(){
                        waitUntil(`#id_photos`,5000).then((el) => storeImages(), (err) => console.log(`img for storing not found`));
                    });
                ");
            }

            if (Globals.IsBuddySystem())
            {

                browser.ExecuteScriptAsync(@"
                        window.addEventListener(`DOMContentLoaded`, function(){
                            waitUntil(`#approve_button`,5000).then( 
                                (el) => document.getElementById(`approve_button`).style.display = `none`,
                                (err) => console.log(`approve not found`));
                        });
                    ");

            }

            Globals.ForceHideComliance = true;
            
            if (Globals.IsServer())
            {
                Globals.ApprovedAgents.Clear();
                ServerAsync.SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count , Url = Globals.CurrentUrl});
                Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
            }
        }

        public void StoreImages(string images)
        {
            this.prevImage = images;
        }

        public void CompareImages(string images, string url)
        {
            Task.Factory.StartNew(() =>
            {
                var prevSha256 = Globals.ComputeSha256Hash(Globals.GetImage(this.prevImage));
                var seedSha256 = Globals.ComputeSha256Hash(Globals.GetImage(images));
                if(prevSha256 != seedSha256)
                {
                    Emailer email = new Emailer();
                    email.subject = "Images not match from seed";
                    email.message = string.Concat("Url: ", url,
                        "\nUser Id: ", Globals.Profile.AgentID,
                        "\nUser name: ", Globals.Profile.Name,
                        "\nSeed sha256: ", seedSha256,
                        "\nPrev sha256: ", prevSha256);
                    email.Send();
                }
            });
        }

        public void DisableForceHide()
        {
            if (Globals.room_duration < Globals.max_room_duration)
            {
                Globals.frmMain.ShowKB();
                return;
            }
                Globals.ForceHideComliance = false;
            browser.EvaluateScriptAsync("$(`#compliance_details,#id_photos`).show()");
        }

        public void showTierLevelBanner(string followRaw)
        {
            Globals.frmMain.showTierLevelWarning(followRaw);
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
                Globals.showMessage(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"));
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
                    UrlInformation urlInfo = Logger.GetUrlInformation(Globals.CurrentUrl);
                    
                        string script = @"
                            var last_room_chatlog = '{{global_chatlog}}';

                            waitUntil('#chatlog_user .chatlog tbody tr', 5000).then( (element) => 
                            { 
                                bound.updateMaxRoomDuration(highlight(element, last_room_chatlog));
                            }, (error) => console.log(error));

                            waitUntil('#data .chatlog tbody tr', 5000).then((element) => highlight(element, last_room_chatlog), (error) => console.log(error));

                            document.getElementById('chatlog_user').addEventListener('DOMSubtreeModified', function()
                            {
                                waitUntil('#chatlog_user .chatlog tbody tr', 5000).then( (element) => {
                                    bound.updateMaxRoomDuration(highlight(element));
                                }, (error) => console.log(error));
                                waitUntil('#data .chatlog tbody tr', 5000).then( (element) => highlight(element), (error) => console.log(error));
                            });


                            console.log('checking images');
                            waitUntil('#data .image_container .image center', 5000).then( (elements) => {
                                elements.forEach( (el) => {
                                    if(el.innerText != '{{last_photo}}')
                                        return;
		                            el.style.background = '#da1b1b';
		                            el.style['color'] = 'white';
                                });
                            }, (error) => console.log(error));

                            waitUntil('#photos .image_container .image center', 5000).then( (elements) => {
                                elements.forEach( (el) => {
                                    if(el.innerText != '{{last_photo}}')
                                        return;
		                            el.style.background = '#da1b1b';
		                            el.style['color'] = 'white';
                                });
                            }, (error) => console.log(error));

                        ";

                        script = script.Replace("{{global_chatlog}}", urlInfo.last_chatlog).Replace("{{last_photo}}", urlInfo.last_photo);
                        browser.ExecuteScriptAsync(script);
                    
                }
                catch {}
            });
        }
        
        public void UpdateMaxRoomDuration(int chatlog_lines_count = 0)
        {
            Console.WriteLine("UpdateMaxRoomDuration: " + chatlog_lines_count);
            if (Globals.IsServer()) {
                ServerAsync.ChatlogRecomputeDurationThreshold(chatlog_lines_count);
                ServerAsync.SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = Globals.max_room_duration.ToString(), RoomDuration = Globals.room_duration });
            }
            else if (Globals.IsClient())
            {
                AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "COMPUTE_TIME", Message = chatlog_lines_count.ToString() });
            }
        }

        public void TriggerClear()
        {
            Globals.frmMain.ButtonClearClick();
        }

        public class HtmlItemClickedEventArgs : EventArgs
        {
            public string Id { get; set; }
        }

        public void SendToIdChecking()
        {   
            if (Globals.ComplianceAgent.id_checking)
            {
                Globals.frmMain.SendToIdChecking();
            }
        }
    }
}
