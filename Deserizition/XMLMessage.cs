using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deserizition
{
    class XMLMessage
    {
        //{"Content":"\u003c?xml version='1.0' encoding='UTF-8' standalone='yes'?\u003e\u003cmsg templateID=\"123\" url=\"http://music.163.com/song?id=492999508\u0026amp;userid=1487080427\u0026amp;from=qq\" serviceID=\"1\" action=\"web\" actionData=\"\" a_actionData=\"\" i_actionData=\"\" brief=\"[分享]分享单曲：和泉紗霧と同棲生活\" flag=\"0\"\u003e\u003citem layout=\"2\"\u003e\u003cpicture cover=\"https://cpic.url.cn/v1/9e85h525uohcnsda7066pspia3phkklu6emk299m5oor3g18u1il44v6e5kpv2rbomcutuc8hj0t3tbkh72mpm6p59kiic1mqfvf7sb88o6tirq8s9ee6t9fn5f8s6cm59mahpempvueu2uukbtarfnijo/7ggqo6k576kp89pk1cei1d8ea6khc37k2bph6p19fj6e2cp9fr10\"/\u003e\u003ctitle\u003e分享单曲：和泉紗霧と同棲生活\u003c/title\u003e\u003csummary\u003e藤田茜/松岡禎丞\u003c/summary\u003e\u003c/item\u003e\u003csource url=\"\" icon=\"\" name=\"网页分享\" appid=\"101793367\" action=\"app\" actionData=\"\" a_actionData=\"tencent101793367://\" i_actionData=\"\"/\u003e\u003c/msg\u003e[分享]分 享单曲：和泉紗霧と同棲生活\n藤田茜/松岡禎丞\nhttp://music.163.com/song?id=492999508\u0026userid=1487080427\u0026from=qq\n来自: 网页分享"}

        public string Content { get; set; }
    }
}
