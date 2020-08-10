using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Launcher.Sdk.Cqp.Core;
using Launcher.Sdk.Cqp.Enum;
using Launcher.Sdk.Cqp.Expand;
using Launcher.Sdk.Cqp.Model;
using Launcher.Sdk.Cqp.Interface;
using Deserizition;

namespace Launcher.Sdk.Cqp
{
	/// <summary>
	/// 表示 酷Q接口 的封装类
	/// </summary>
	public class CQApi
	{
		#region --CQ码类方法--
		/// <summary>
		/// 获取酷Q "At某人" 代码
		/// </summary>
		/// <param name="qqId">QQ号</param>
		/// <exception cref="ArgumentOutOfRangeException">参数: qqId 超出范围</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_At (long qqId)
		{
			if (qqId < 0)
			{
				throw new ArgumentOutOfRangeException ("qqId");
			}
			return new CQCode (
				CQFunction.At,
				new KeyValuePair<string, string> ("qq", Convert.ToString (qqId)));
		}
		/// <summary>
		/// 获取酷Q "At全体成员" 代码
		/// </summary>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_AtAll ()
		{
			return new CQCode (
				CQFunction.At,
				new KeyValuePair<string, string> ("qq", "all"));
		}
		/// <summary>
		/// 获取酷Q "Emoji" 代码
		/// </summary>
		/// <param name="id">Emoji的Id</param>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Emoji (int id)
		{
			return new CQCode (
				CQFunction.Emoji,
				new KeyValuePair<string, string> ("id", Convert.ToString (id)));
		}
		/// <summary>
		/// 获取酷Q "表情" 代码
		/// </summary>
		/// <param name="face">表情枚举</param>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Face (CQFace face)
		{
			return new CQCode (
				CQFunction.Face,
				new KeyValuePair<string, string> ("id", Convert.ToString ((int)face)));
		}
		/// <summary>
		/// 获取酷Q "戳一戳" 代码
		/// </summary>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Shake ()
		{
			return new CQCode (CQFunction.Shake);
		}
		/// <summary>
		/// 获取字符串副本的转义形式
		/// </summary>
		/// <param name="source">欲转义的原始字符串</param>
		/// <param name="enCodeComma">是否转义逗号, 默认 <code>false</code></param>
		/// <exception cref="ArgumentNullException">参数: source 为 null</exception>
		/// <returns>返回转义后的字符串副本</returns>
		public static string CQEnCode (string source, bool enCodeComma)
		{
			if (source == null)
			{
				throw new ArgumentNullException ("source");
			}
			StringBuilder builder = new StringBuilder (source);
			builder = builder.Replace ("&", "&amp;");
			builder = builder.Replace ("[", "&#91;");
			builder = builder.Replace ("]", "&#93;");
			if (enCodeComma)
			{
				builder = builder.Replace (",", "&#44;");
			}
			return builder.ToString ();
		}
		/// <summary>
		/// 获取字符串副本的非转义形式
		/// </summary>
		/// <param name="source">欲反转义的原始字符串</param>
		/// <exception cref="ArgumentNullException">参数: source 为 null</exception>
		/// <returns>返回反转义的字符串副本</returns>
		public static string CQDeCode (string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException ("source");
			}
			StringBuilder builder = new StringBuilder (source);
			builder = builder.Replace ("&#91;", "[");
			builder = builder.Replace ("&#93;", "]");
			builder = builder.Replace ("&#44;", ",");
			builder = builder.Replace ("&amp;", "&");
			return builder.ToString ();
		}
		/// <summary>
		/// 获取酷Q "链接分享" 代码
		/// </summary>
		/// <param name="url">分享的链接</param>
		/// <param name="title">显示的标题, 建议12字以内</param>
		/// <param name="content">简介信息, 建议30字以内</param>
		/// <param name="imageUrl">分享的图片链接, 留空则为默认图片</param>
		/// <exception cref="ArgumentException">参数: url 是空字符串或为 null</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_ShareLink (string url, string title, string content, string imageUrl = null)
		{
			if (string.IsNullOrEmpty (url))
			{
				throw new ArgumentException ("分享的链接为空", "url");
			}

			CQCode code = new CQCode (
				CQFunction.Share,
				new KeyValuePair<string, string> ("url", url));

			if (!string.IsNullOrEmpty (title))
			{
				code.Items.Add ("title", title);
			}
			if (!string.IsNullOrEmpty (content))
			{
				code.Items.Add ("content", content);
			}
			if (!string.IsNullOrEmpty (imageUrl))
			{
				code.Items.Add ("image", imageUrl);
			}

			return code;
		}
		/// <summary>
		/// 获取酷Q "位置分享" 代码
		/// </summary>
		/// <param name="site">地点, 建议12字以内</param>
		/// <param name="detail">详细地址, 建议20字以内</param>
		/// <param name="lat">维度</param>
		/// <param name="lon">经度</param>
		/// <param name="zoom">放大倍数, 默认: 15倍</param>
		/// <exception cref="ArgumentException">参数: site 或 detail 是空字符串或为 null</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_ShareGPS (string site, string detail, double lat, double lon, int zoom = 15)
		{
			if (string.IsNullOrEmpty (site))
			{
				throw new ArgumentException ("分享的地点不能为空", "site");
			}

			if (string.IsNullOrEmpty (detail))
			{
				throw new ArgumentException ("详细地址不能为空", "detail");
			}

			return new CQCode (
				CQFunction.Location,
				new KeyValuePair<string, string> ("lat", Convert.ToString (lat)),
				new KeyValuePair<string, string> ("lon", Convert.ToString (lon)),
				new KeyValuePair<string, string> ("zoom", Convert.ToString (zoom)),
				new KeyValuePair<string, string> ("title", site),
				new KeyValuePair<string, string> ("content", detail));
		}
		/// <summary>
		/// 获取酷Q "匿名" 代码
		/// </summary>
		/// <param name="forced">强制发送, 若本参数为 <code>true</code> 发送失败时将转换为普通消息</param>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Anonymous (bool forced = false)
		{
			CQCode code = new CQCode (CQFunction.Anonymous);
			if (forced)
			{
				code.Items.Add ("ignore", "true");
			}
			return code;
		}
		/// <summary>
		/// 获取酷Q "音乐" 代码
		/// </summary>
		/// <param name="id"></param>
		/// <param name="type"></param>
		/// <param name="style"></param>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Music (long id, CQMusicType type = CQMusicType.Tencent, CQMusicStyle style = CQMusicStyle.Old)
		{
			return new CQCode (
				CQFunction.Music,
				new KeyValuePair<string, string> ("id", Convert.ToString (id)),
				new KeyValuePair<string, string> ("type", type.GetDescription ()),
				new KeyValuePair<string, string> ("style", Convert.ToString ((int)style)));
		}
		/// <summary>
		/// 获取酷Q "音乐自定义" 代码
		/// </summary>
		/// <param name="url">分享链接, 点击后进入的页面 (歌曲介绍)</param>
		/// <param name="musicUrl">歌曲链接, 音频链接 (mp3链接)</param>
		/// <param name="title">标题, 建议12字以内</param>
		/// <param name="content">简介, 建议30字以内</param>
		/// <param name="imageUrl">封面图片链接, 留空为默认</param>
		/// <exception cref="ArgumentException">参数: url 或 musicUrl 是空字符串或为 null</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_DIYMusic (string url, string musicUrl, string title = null, string content = null, string imageUrl = null)
		{
			if (string.IsNullOrEmpty (url))
			{
				throw new ArgumentException ("分享链接不能为空", "url");
			}

			if (string.IsNullOrEmpty (musicUrl))
			{
				throw new ArgumentException ("歌曲链接不能为空", "musicUrl");
			}

			CQCode code = new CQCode (
				CQFunction.Music,
				new KeyValuePair<string, string> ("type", "custom"),
				new KeyValuePair<string, string> ("url", url),
				new KeyValuePair<string, string> ("audio", musicUrl));
			if (!string.IsNullOrEmpty (title))
			{
				code.Items.Add ("title", title);
			}

			if (!string.IsNullOrEmpty (content))
			{
				code.Items.Add ("content", content);
			}

			if (!string.IsNullOrEmpty (imageUrl))
			{
				code.Items.Add ("imageUrl", imageUrl);
			}
			return code;
		}
		/// <summary>
		/// 获取酷Q "图片" 代码
		/// </summary>
		/// <param name="path">图片的路径, 将图片放在 酷Q\data\image 下, 并填写相对路径. 如 酷Q\data\image\1.jpg 则填写 1.jpg</param>
		/// <exception cref="ArgumentException">参数: path 是空字符串或为 null</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Image (string path)
		{
			if (string.IsNullOrEmpty (path))
			{
				throw new ArgumentException ("路径不能为空", "path");
			}

			return new CQCode (
				CQFunction.Image,
				new KeyValuePair<string, string> ("file", path));
		}
		/// <summary>
		/// 获取酷Q "语音" 代码
		/// </summary>
		/// <param name="path">语音的路径, 将音频放在 酷Q\data\record 下, 并填写相对路径. 如 酷Q\data\record\1.amr 则填写 1.amr</param>
		/// <exception cref="ArgumentException">参数: path 是空字符串或为 null</exception>
		/// <returns>返回 <see cref="CQCode"/> 对象</returns>
		public static CQCode CQCode_Record (string path)
		{
			if (string.IsNullOrEmpty (path))
			{
				throw new ArgumentException ("语音路径不允许为空", "path");
			}

			return new CQCode (
				CQFunction.Record,
				new KeyValuePair<string, string> ("file", path));
		}
		#endregion
	}
}