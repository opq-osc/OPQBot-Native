﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Deserizition;
using System.Diagnostics;

namespace Launcher.Tests
{
    [TestClass()]
    public class ProgressMessageTests
    {
        [TestMethod()]
        public void StartTest()
        {
            string json = "{\r\n  \"CurrentPacket\": {\r\n    \"WebConnId\": \"\",\r\n    \"Data\": {\r\n      \"FromGroupId\": 891787846,\r\n      \"FromGroupName\": \"水银控♂制台\",\r\n      \"FromUserId\": 863450594,\r\n      \"FromNickName\": \"(*´◐∀◐`*)智障\",\r\n      \"Content\": \"{\\\"Content\\\":\\\"[[分享]寄世界于东方～Otomad World]请使用最新版本手机QQ查看{\\\\\\\"app\\\\\\\":\\\\\\\"com.tencent.structmsg\\\\\\\",\\\\\\\"desc\\\\\\\":\\\\\\\"音乐\\\\\\\",\\\\\\\"view\\\\\\\":\\\\\\\"music\\\\\\\",\\\\\\\"ver\\\\\\\":\\\\\\\"0.0.0.1\\\\\\\",\\\\\\\"prompt\\\\\\\":\\\\\\\"[分享]寄世界于东方～Otomad World\\\\\\\",\\\\\\\"meta\\\\\\\":{\\\\\\\"music\\\\\\\":{\\\\\\\"action\\\\\\\":\\\\\\\"\\\\\\\",\\\\\\\"android_pkg_name\\\\\\\":\\\\\\\"\\\\\\\",\\\\\\\"app_type\\\\\\\":1,\\\\\\\"appid\\\\\\\":100495085,\\\\\\\"desc\\\\\\\":\\\\\\\"玛丽的对头\\\\\\\\/Snowf_Ake\\\\\\\",\\\\\\\"jumpUrl\\\\\\\":\\\\\\\"https:\\\\\\\\/\\\\\\\\/y.music.163.com\\\\\\\\/m\\\\\\\\/song\\\\\\\\/1421345210\\\\\\\\/?userid=1487080427\\\\\\\",\\\\\\\"musicUrl\\\\\\\":\\\\\\\"http:\\\\\\\\/\\\\\\\\/music.163.com\\\\\\\\/song\\\\\\\\/media\\\\\\\\/outer\\\\\\\\/url?id=1421345210\\\\u0026userid=1487080427\\\\\\\",\\\\\\\"preview\\\\\\\":\\\\\\\"http:\\\\\\\\/\\\\\\\\/p3.music.126.net\\\\\\\\/m5jfcQrNF3KIBUGS0z3Yag==\\\\\\\\/109951164682072553.jpg\\\\\\\",\\\\\\\"sourceMsgId\\\\\\\":\\\\\\\"0\\\\\\\",\\\\\\\"source_icon\\\\\\\":\\\\\\\"\\\\\\\",\\\\\\\"source_url\\\\\\\":\\\\\\\"\\\\\\\",\\\\\\\"tag\\\\\\\":\\\\\\\"网易云音乐\\\\\\\",\\\\\\\"title\\\\\\\":\\\\\\\"寄世界于东方～Otomad World\\\\\\\"}},\\\\\\\"config\\\\\\\":{\\\\\\\"autosize\\\\\\\":true,\\\\\\\"ctime\\\\\\\":1624234735,\\\\\\\"forward\\\\\\\":true,\\\\\\\"token\\\\\\\":\\\\\\\"67f82d769d8ffd790fe7378466fc3915\\\\\\\",\\\\\\\"type\\\\\\\":\\\\\\\"normal\\\\\\\"}}\\\"}\",\r\n      \"MsgType\": \"JsonMsg\",\r\n      \"MsgTime\": 1624235276,\r\n      \"MsgSeq\": 23533,\r\n      \"MsgRandom\": 1413885862,\r\n      \"RedBaginfo\": null\r\n    }\r\n  },\r\n  \"CurrentQQ\": 3020887057\r\n}";
            string xml = "{\r\n  \"CurrentPacket\": {\r\n    \"WebConnId\": \"\",\r\n    \"Data\": {\r\n      \"FromGroupId\": 891787846,\r\n      \"FromGroupName\": \"水银控♂制台\",\r\n      \"FromUserId\": 863450594,\r\n      \"FromNickName\": \"(*´◐∀◐`*)智障\",\r\n      \"Content\": \"{\\\"Content\\\":\\\"\\\\u003c?xml version='1.0' encoding='UTF-8' standalone='yes' ?\\\\u003e\\\\u003cmsg serviceID=\\\\\\\"35\\\\\\\" templateID=\\\\\\\"1\\\\\\\" action=\\\\\\\"viewMultiMsg\\\\\\\" brief=\\\\\\\"[聊天记录]\\\\\\\" m_resid=\\\\\\\"u4+3DkbZM5n+XAwOUlDdiQI94jzboPgkAFp2CTFDr/VJDeCb4n9KwZRbof37CpvY\\\\\\\" m_fileName=\\\\\\\"6976041871604282006\\\\\\\" tSum=\\\\\\\"2\\\\\\\" sourceMsgId=\\\\\\\"0\\\\\\\" url=\\\\\\\"\\\\\\\" flag=\\\\\\\"3\\\\\\\" adverSign=\\\\\\\"0\\\\\\\" multiMsgFlag=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003citem layout=\\\\\\\"1\\\\\\\" advertiser_id=\\\\\\\"0\\\\\\\" aid=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003ctitle size=\\\\\\\"34\\\\\\\" maxLines=\\\\\\\"2\\\\\\\" lineSpace=\\\\\\\"12\\\\\\\"\\\\u003e群聊的聊天记录\\\\u003c/title\\\\u003e\\\\u003ctitle size=\\\\\\\"26\\\\\\\" color=\\\\\\\"#777777\\\\\\\" maxLines=\\\\\\\"4\\\\\\\" lineSpace=\\\\\\\"12\\\\\\\"\\\\u003epoweroff:  老民科了\\\\u003c/title\\\\u003e\\\\u003ctitle size=\\\\\\\"26\\\\\\\" color=\\\\\\\"#777777\\\\\\\" maxLines=\\\\\\\"4\\\\\\\" lineSpace=\\\\\\\"12\\\\\\\"\\\\u003e18车卓汤伟诚:  其实和我们不是同一个系的，是石油的，没什么了解🤔\\\\u003c/title\\\\u003e\\\\u003chr hidden=\\\\\\\"false\\\\\\\" style=\\\\\\\"0\\\\\\\" /\\\\u003e\\\\u003csummary size=\\\\\\\"26\\\\\\\" color=\\\\\\\"#777777\\\\\\\"\\\\u003e查看2条转发消息\\\\u003c/summary\\\\u003e\\\\u003c/item\\\\u003e\\\\u003csource name=\\\\\\\"聊天记录\\\\\\\" icon=\\\\\\\"\\\\\\\" action=\\\\\\\"\\\\\\\" appid=\\\\\\\"-1\\\\\\\" /\\\\u003e\\\\u003c/msg\\\\u003e\\\"}\",\r\n      \"MsgType\": \"XmlMsg\",\r\n      \"MsgTime\": 1624236319,\r\n      \"MsgSeq\": 23534,\r\n      \"MsgRandom\": 417139587,\r\n      \"RedBaginfo\": null\r\n    }\r\n  },\r\n  \"CurrentQQ\": 3020887057\r\n}";
            string qquser = "{\r\n  \"CurrentPacket\": {\r\n    \"WebConnId\": \"\",\r\n    \"Data\": {\r\n      \"FromGroupId\": 891787846,\r\n      \"FromGroupName\": \"水银控♂制台\",\r\n      \"FromUserId\": 863450594,\r\n      \"FromNickName\": \"(*´◐∀◐`*)智障\",\r\n      \"Content\": \"{\\\"Content\\\":\\\"\\\\u003c?xml version='1.0' encoding='UTF-8' standalone='yes' ?\\\\u003e\\\\u003cmsg serviceID=\\\\\\\"14\\\\\\\" templateID=\\\\\\\"1\\\\\\\" action=\\\\\\\"plugin\\\\\\\" actionData=\\\\\\\"AppCmd://OpenContactInfo/?uin=2185367837\\\\\\\" a_actionData=\\\\\\\"mqqapi://card/show_pslcard?src_type=internal\\\\u0026amp;source=sharecard\\\\u0026amp;version=1\\\\u0026amp;uin=2185367837\\\\\\\" i_actionData=\\\\\\\"mqqapi://card/show_pslcard?src_type=internal\\\\u0026amp;source=sharecard\\\\u0026amp;version=1\\\\u0026amp;uin=2185367837\\\\\\\" brief=\\\\\\\"推荐了水银之翼\\\\\\\" sourceMsgId=\\\\\\\"0\\\\\\\" url=\\\\\\\"\\\\\\\" flag=\\\\\\\"1\\\\\\\" adverSign=\\\\\\\"0\\\\\\\" multiMsgFlag=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003citem layout=\\\\\\\"0\\\\\\\" mode=\\\\\\\"1\\\\\\\" advertiser_id=\\\\\\\"0\\\\\\\" aid=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003csummary\\\\u003e推荐联系人\\\\u003c/summary\\\\u003e\\\\u003chr hidden=\\\\\\\"false\\\\\\\" style=\\\\\\\"0\\\\\\\" /\\\\u003e\\\\u003c/item\\\\u003e\\\\u003citem layout=\\\\\\\"2\\\\\\\" mode=\\\\\\\"1\\\\\\\" advertiser_id=\\\\\\\"0\\\\\\\" aid=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003cpicture cover=\\\\\\\"mqqapi://card/show_pslcard?src_type=internal\\\\u0026amp;source=sharecard\\\\u0026amp;version=1\\\\u0026amp;uin=2185367837\\\\\\\" w=\\\\\\\"0\\\\\\\" h=\\\\\\\"0\\\\\\\" /\\\\u003e\\\\u003ctitle\\\\u003e水银之翼\\\\u003c/title\\\\u003e\\\\u003csummary\\\\u003e帐号:2185367837\\\\u003c/summary\\\\u003e\\\\u003c/item\\\\u003e\\\\u003csource name=\\\\\\\"\\\\\\\" icon=\\\\\\\"\\\\\\\" action=\\\\\\\"\\\\\\\" appid=\\\\\\\"-1\\\\\\\" /\\\\u003e\\\\u003c/msg\\\\u003e你的QQ暂不支持查看推荐好友，请期待后续版本。\\\"}\",\r\n      \"MsgType\": \"XmlMsg\",\r\n      \"MsgTime\": 1624236319,\r\n      \"MsgSeq\": 23534,\r\n      \"MsgRandom\": 417139587,\r\n      \"RedBaginfo\": null\r\n    }\r\n  },\r\n  \"CurrentQQ\": 3020887057\r\n}";
            string group = "{\r\n  \"CurrentPacket\": {\r\n    \"WebConnId\": \"\",\r\n    \"Data\": {\r\n      \"FromGroupId\": 891787846,\r\n      \"FromGroupName\": \"水银控♂制台\",\r\n      \"FromUserId\": 863450594,\r\n      \"FromNickName\": \"(*´◐∀◐`*)智障\",\r\n      \"Content\": \"{\\\"Content\\\":\\\"\\\\u003c?xml version='1.0' encoding='UTF-8' standalone='yes' ?\\\\u003e\\\\u003cmsg serviceID=\\\\\\\"15\\\\\\\" templateID=\\\\\\\"1\\\\\\\" action=\\\\\\\"web\\\\\\\" actionData=\\\\\\\"group:891787846\\\\\\\" a_actionData=\\\\\\\"group:891787846\\\\\\\" i_actionData=\\\\\\\"group:891787846\\\\\\\" brief=\\\\\\\"推荐群聊：水银控♂制台\\\\\\\" sourceMsgId=\\\\\\\"0\\\\\\\" url=\\\\\\\"https://jq.qq.com/?_wv=1027\\\\u0026amp;k=ivsIdSoV\\\\\\\" flag=\\\\\\\"0\\\\\\\" adverSign=\\\\\\\"0\\\\\\\" multiMsgFlag=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003citem layout=\\\\\\\"0\\\\\\\" mode=\\\\\\\"1\\\\\\\" advertiser_id=\\\\\\\"0\\\\\\\" aid=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003csummary\\\\u003e推荐群聊\\\\u003c/summary\\\\u003e\\\\u003chr hidden=\\\\\\\"false\\\\\\\" style=\\\\\\\"0\\\\\\\" /\\\\u003e\\\\u003c/item\\\\u003e\\\\u003citem layout=\\\\\\\"2\\\\\\\" mode=\\\\\\\"1\\\\\\\" advertiser_id=\\\\\\\"0\\\\\\\" aid=\\\\\\\"0\\\\\\\"\\\\u003e\\\\u003cpicture cover=\\\\\\\"http://p.qlogo.cn/gh/891787846/891787846/100\\\\\\\" w=\\\\\\\"0\\\\\\\" h=\\\\\\\"0\\\\\\\" needRoundView=\\\\\\\"0\\\\\\\" /\\\\u003e\\\\u003ctitle\\\\u003e水银控♂制台\\\\u003c/title\\\\u003e\\\\u003csummary\\\\u003e水银之翼天下第 一!\\\\u003c/summary\\\\u003e\\\\u003c/item\\\\u003e\\\\u003csource name=\\\\\\\"\\\\\\\" icon=\\\\\\\"\\\\\\\" action=\\\\\\\"\\\\\\\" appid=\\\\\\\"-1\\\\\\\" /\\\\u003e\\\\u003c/msg\\\\u003ehttps://jq.qq.com/?_wv=1027\\\\u0026k=ivsIdSoV\\\"}\",\r\n      \"MsgType\": \"XmlMsg\",\r\n      \"MsgTime\": 1624236319,\r\n      \"MsgSeq\": 23534,\r\n      \"MsgRandom\": 417139587,\r\n      \"RedBaginfo\": null\r\n    }\r\n  },\r\n  \"CurrentQQ\": 3020887057\r\n}";
            var b = ProgressMessage.Start(JsonConvert.DeserializeObject<ReceiveMessage>(json));
            Debug.WriteLine(b);
        }
    }
}