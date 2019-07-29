using CefSharp;
using CefSharp.WinForms;
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
                browser.EvaluateScriptAsync(@"window.onmousemove = function(e) { bound.onBrowserEvent(); }");
                if (e.Frame.Url.Contains(Globals.CB_COMPLIANCE_SET_ID_EXP_URL))
                {
                    var submit_script = @"
                        var set_expr = document.querySelectorAll(`input[value='Update Expiration Date']`)[0];
                        var expr_date = setInterval(function(){
                            if(set_expr != undefined){
                                console.log('EXPR binded');
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
                    var violation_interval = setInterval(function(){
                        if(document.getElementById('violation-submit') != undefined){
                            console.log('VR binded');
                            document.getElementById('violation-submit').addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(violation_interval);
                        }
                    }, 1000);
 
                    var id_missing_interval = setInterval(function(){
                        var id_missing = document.querySelectorAll(`input[value='Report Identification Missing Problem']`)[0];
                        if(id_missing != undefined){
                            console.log('IM binded');
                            id_missing.addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked('id-missing');
                            },false)
                            clearInterval(id_missing_interval);
                        }
                    }, 1000);

                    var spammer_interval = setInterval(function(){
                        if($('#spammer-submit')[0] != undefined){
                            console.log('SR binded');
                            $('#spammer-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(spammer_interval);
                        }
                    }, 1000)

                    var request_review_interval = setInterval(function(){
                        if($('#request-review-submit')[0] != undefined){
                            console.log('RR binded');
                            $('#request-review-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_interval);
                        }
                    }, 1000);

                    var bs_agree = setInterval(function(){
                        if($('#agree_button')[0] != undefined){
                            console.log('BSA binded');
                            $('#agree_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_agree);
                        }
                    }, 1000);

                    var bs_disagree = setInterval(function(){
                        if($('#disagree_button')[0] != undefined){
                            console.log('BSD binded');
                            $('#disagree_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_disagree);
                        }
                    }, 1000);

                    var request_review_reply = setInterval(function(){
                        if($('#reply_button')[0] != undefined){
                            console.log('RRR binded');
                            $('#reply_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_reply);
                        }
                    }, 1000);

                    var change_gender = setInterval(function(){
                        var chg = document.querySelectorAll(`input[value='Change Gender']`)[0]; 
                        if (chg != undefined){
                            console.log('CG binded');
                            chg.addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked('change_gender');
                            },false)
                            clearInterval(change_gender);
                        }
                    }, 1000);

                    $('#approve_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                    
                    var bounce = $(`body:contains('locked to another bouncer')`).length;
                    if(bounce > 0){
                        bound.saveAsBounce();
                    }
                       
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
