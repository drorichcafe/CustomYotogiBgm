
# CM3D2.CustomYotogiBgm.Plugin

夜伽のコマンド実行時にBGMを鳴らすプラグインです。OGGファイルのみ対応しています。

ConfigフォルダにあるCustomYotogiBgm.xmlを編集して条件を記述してください。Itemタグには下記タグを記述することができます。条件になるタグを記述していないパラメータについては判定を行いません。

| タグ名           | 概要             | 補足                                                        |
|:-----------------|:-----------------|:------------------------------------------------------------|
| SkillCategory    | カテゴリ名       | 「淫欲」等                                                  |
| SkillName        | スキル名         | 「セックス正常位」等                                        |
| CommandName      | コマンド名       | 「責める」等　　　　　                                      |
| CommandType      | コマンドタイプ   | 挿入, 継続, 単発, 単発_挿入, 絶頂, 止める                   |
| ExcitementStatus | 興奮値の状態遷移 | Minus, Small, Medium, Large                                 |
| StageName        | ステージ名       | 「トイレ」等                                                |
| Files            | ファイルリスト   | 子要素にFileタグを記述してください                          |
| └ File          | 再生ファイル     | 複数の場合ランダム再生                                      |
| 　 ├ Name       | ファイル名       | 再生するOGGファイル名                                       |
| 　 ├ Loop       | ループ再生       | true or false                                               |
| 　 └ FadeTime   | フェード時間     | BGMをフェードで切り替える時間（秒）                         |

パラメータが文字列の場合は部分一致が有効になっています。例えば&lt;SkillName&gt;ポーズ&lt;/SkillName&gt;と記述した場合は、「かっこいいポーズ」でも「かわいいポーズ」でも条件にマッチします。

OGGファイルはGameData以下（Sybarisの場合はSybaris\GameData以下）に置いてください。
条件に合うBGMが見つからなかった場合はステージのデフォルトBGMが再生されます。

#### CM3D2.CustomYotogiBgmStatic

夜伽ステージのデフォルトBGMを変更することができます。
ConfigフォルダにあるCustomYotogiBgmStatic.xmlを編集してください。

### 更新履歴

* 0.0.0.3
  * CustomYotogiBgmStaticにSpecific、Generalの設定を追加

* 0.0.0.2
  * WaitCommandの仕様を削除
  * FadeTimeタグを追加
  * CM3D2.CustomYotogiBgmStaticプラグインを追加

* 0.0.0.1
  * ステージ選択画面でBGMが切り替わってしまう不具合を修正
  * ステージに設定されたものをデフォルトBGMとして使用するように修正

@drorichcafe
