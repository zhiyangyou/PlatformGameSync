/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/6/10 15:59:22
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class LoadingGameWindowUIComponent
	{
		public   Text  txtLoadingProgressText;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     txtLoadingProgressText =target.transform.Find("BG/[Text]txtLoadingProgress").GetComponent<Text>();
	
	
		     //组件事件绑定
		     LoadingGameWindow mWindow=(LoadingGameWindow)target;
		}
	}
}
