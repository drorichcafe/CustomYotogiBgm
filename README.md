# CM3D2.CustomYotogiBgm.Plugin

夜伽の条件でBGMを変更するプラグインです。OGGファイルのみ対応しています。

ConfigフォルダにあるCustomYotogiBgm.xmlを編集して条件を記述してください。Itemタグには下記タグを記述することができます。条件になるタグを記述していないパラメータについては判定を行いません。

| タグ名           | 概要             | 補足                                                        |
|:-----------------|:-----------------|:------------------------------------------------------------|
| SkillCategory    | カテゴリ名       | 「淫欲」等                                                  |
| SkillName        | スキル名         | 「セックス正常位」等                                        |
| WaitCommand      | コマンド実行待ち | true, false                                                 |
| CommandName      | コマンド名       | 「責める」等　　　　　                                      |
| CommandType      | コマンドタイプ   | 挿入, 継続, 単発, 単発_挿入, 絶頂, 止める                   |
| ExcitementStatus | 興奮値の状態遷移 | Minus, Small, Medium, Large                                 |
| StageName        | ステージ名       | 「トイレ」等                                                |
| Files            | ファイルリスト   | 子要素にFileタグを記述してください                          |
| File             | 再生ファイル     | 複数の場合ランダム再生。Nameにファイル名、Loopはtrueかfalse |

パラメータが文字列の場合は部分一致が有効になっています。例えば&lt;SkillName&gt;ポーズ&lt;/SkillName&gt;と記述した場合は、「かっこいいポーズ」でも「かわいいポーズ」でも条件にマッチします。

OGGファイルはGameData以下（Sybarisの場合はSybaris\GameData以下）に置いてください。
条件に合うBGMが見つからなかった場合はステージのデフォルトBGMが再生されます。

### 既知の不具合

* スキル開始時、WaitCommandがfalseの音楽がステージのデフォルトBGMで上書きされてしまいます

### 更新履歴

* 0.0.0.1
  * ステージ選択画面でBGMが切り替わってしまう不具合を修正
  * ステージに設定されたものをデフォルトBGMとして使用するように修正

@drorichcafe
