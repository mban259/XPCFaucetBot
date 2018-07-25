using System;
using System.Collections.Generic;
using System.Text;

namespace XPCFaucetBot.Utils
{
    static class Messages
    {
        //0 メンション,
        internal static string[] VoiceChatJoinMessages = new string[]
        {
            "{0}さんいらっしゃい",
            "{0}さん、いらっしゃ～い",
            "、、、ん！？　そうか！そこで {0} か！！",
            "「まだ {0} してないの？」"
        };

        internal const string DirectMessageReturnText = @"仕事忍satoshiは有人の操作ではなく、自動で動作するプログラム（BOT)です。
XPCに関する質問に関しては、<#447671198566973480>へお願いします。
なお<#447671198566973480>はボランティアの方が回答する場合もありますので、お手数ですが質問の際は「何が起きていて、何で困っているのか」を記載頂けると助かります。";
        //{0}メンション
        //{1}通貨名
        internal const string SignMessageReturnText = @"{0} 🈲 ここで!{1} message signコマンドは使わないでください 🈲

!{1} message signは個人の証明に使うコマンドですので、<@441117179405008897>へのDM専用となっています。
ここで!{1} message signコマンドを打っても無効です。
PCの場合、画面右側、ユーザー一覧から「CCWallet」を右クリックしてメッセージを選択すると、DMの画面に移ります。
スマホの場合は、チャット画面を左にスワイプすればユーザー一覧が表示されますので、「CCWallet」をタップしてメッセージを選択してください。";
    }
}
