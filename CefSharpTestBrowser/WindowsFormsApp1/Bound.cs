using CefSharp;
using CefSharp.WinForms;
using SkydevCSTool.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                browser.EvaluateScriptAsync(@"
                    var bounce = $(`body:contains('locked to another bouncer')`).length;
                    if(bounce > 0){
                        bound.saveAsBounce();
                    }
                ");
            }
        }
        public void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                browser.EvaluateScriptAsync(@"window.onmousemove = function(e) { bound.onBrowserEvent(); }");
                if (e.Frame.Url.Contains(Url.CB_COMPLIANCE_SET_ID_EXP_URL))
                {
                    var submit_script = @"
                        var set_expr = document.querySelectorAll(`input[value='Update Expiration Date']`)[0];
                        var expr_date = setInterval(function(){
                            if(set_expr != undefined){
                                console.log('EXPR binded', window.location.href);
                                set_expr.addEventListener('click', 
                                function(e)
                                {
                                    bound.onClicked('set_expr');
                                },false)
                                clearInterval(expr_date);
                            }
                        }, 1000);
                    ";
                    browser.EvaluateScriptAsync(@submit_script);
                }
                else
                {
                    var submit_script = @"
                   
                    var approve_interval = setInterval(function(){
                        if(document.getElementById('approve_button') != undefined){
                            console.log('AP binded', window.location.href);
                            document.getElementById('approve_button').addEventListener('click', 
                            function(e)
                            {
                                console.log('AP clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(approve_interval);
                        }
                    }, 1000);
                    
                    var violation_interval = setInterval(function(){
                        if(document.getElementById('violation-submit') != undefined){
                            console.log('VR binded', window.location.href);
                            document.getElementById('violation-submit').addEventListener('click', 
                            function(e)
                            {
                                console.log('VR clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(violation_interval);
                        }
                    }, 1000);
 
                    var id_missing_interval = setInterval(function(){
                        var id_missing = document.querySelectorAll(`input[value='Report Identification Missing Problem']`)[0];
                        if(id_missing != undefined){
                            console.log('IM binded', window.location.href);
                            id_missing.addEventListener('click', 
                            function(e)
                            {
                                console.log('IM clicked');
                                bound.onClicked('id-missing');
                            },false)
                            clearInterval(id_missing_interval);
                        }
                    }, 1000);

                    var spammer_interval = setInterval(function(){
                        if(document.getElementById('spammer-submit') != undefined){
                            console.log('SR binded', window.location.href);
                            document.getElementById('spammer-submit').addEventListener('click', 
                            function(e)
                            {
                                console.log('SR clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(spammer_interval);
                        }
                    }, 1000)

                    var request_review_interval = setInterval(function(){
                        if(document.getElementById('request-review-submit') != undefined){
                            console.log('RR binded', window.location.href);
                            document.getElementById('request-review-submit').addEventListener('click', 
                            function(e)
                            {
                                console.log('RR clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_interval);
                        }
                    }, 1000);

                    var bs_agree = setInterval(function(){
                        if(document.getElementById('agree_button') != undefined){
                            console.log('BSA binded', window.location.href);
                            document.getElementById('agree_button').addEventListener('click', 
                            function(e)
                            {
                                console.log('BSA clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_agree);
                        }
                    }, 1000);

                    var bs_disagree = setInterval(function(){
                        if(document.getElementById('disagree_button') != undefined){
                            console.log('BSD binded', window.location.href);
                            document.getElementById('agree_button').addEventListener('click', 
                            function(e)
                            {
                                console.log('BSD clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_disagree);
                        }
                    }, 1000);

                    var request_review_reply = setInterval(function(){
                        if(document.getElementById('reply_button') != undefined){
                            console.log('RRR binded', window.location.href);
                            document.getElementById('reply_button').addEventListener('click', 
                            function(e)
                            {
                                console.log('RRR clicked');
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_reply);
                        }
                    }, 1000);

                    var change_gender = setInterval(function(){
                        var chg = document.querySelectorAll(`input[value='Change Gender']`)[0]; 
                        if (chg != undefined){
                            console.log('CG binded', window.location.href);
                            chg.addEventListener('click', 
                            function(e)
                            {
                                console.log('CG clicked');
                                bound.onClicked('change_gender');
                            },false)
                            clearInterval(change_gender);
                        }
                    }, 1000);
                    
                    
                    
                    
                       
                    var zoomLevel = 1;
                    window.addEventListener('wheel', function(e) {
                        if (e.deltaY < 0) {
                             if(event.ctrlKey && zoomLevel < 2){
                                    console.log('scrolling up');
                                    zoomLevel += 0.1;
                                     document.body.style.zoom = zoomLevel;
                             };        
                        }
                        
                        if (e.deltaY > 0) {
                           if(event.ctrlKey && zoomLevel > 0.5){
                                    console.log('scrolling down');
                                    zoomLevel -= 0.1;
                                    document.body.style.zoom = zoomLevel;
                             }; 
                             
                        }
                    });


";
                    browser.EvaluateScriptAsync(@submit_script);
                }
            }
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
