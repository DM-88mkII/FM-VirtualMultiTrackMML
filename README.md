# FM-VirtualMultiTrackMML
OPN系の効果音モードを使わずに、通常チャンネルで2トラックを鳴らす為のラッパーMMLです。  
現在は、[MUCOM88](https://onitama.tv/mucom88/) への変換にしか対応していませんが、もし他のMMLツールで使ってみたいというご要望があれば、前向きに検討します。  

実行ファイルのダウンロードは[こちら](https://github.com/DM-88mkII/FM-VirtualMultiTrackMML/blob/main/FM-VirtualMultiTrackMML/bin/Release/FM-VirtualMultiTrackMML.exe)  

<br>

# 使い方
~~~
FM-VirtualMultiTrackMML.exe 入力ファイル

エラーが無ければ、変換後の文字列がクリップボードにコピーされます。
~~~

<br>

# 書式
[MUCOM88](https://onitama.tv/mucom88/) に似せた書式になっています。  
~~~
;設定
#OCTAVE_UPDOWN [><|<>]
#VOLUME_UPDOWN [)(|()]

;音色定義
  @No:{
  FB
  AR1, DR1, SR1, RR1, SL1, TL1, KS1, SE1
  AR2, DR2, SR2, RR2, SL2, TL2, KS2, SE2, "Name"}

;マクロ定義
# *n{}

;トラック定義
A チャンネル0の主トラック
a チャンネル0の副トラック
B チャンネル1の主トラック
b チャンネル1の副トラック
:
Z チャンネル25の主トラック
z チャンネル25の副トラック
~~~
26チャンネルまで記述可能ですが、6チャンネル分しか変換しません。  

* aliasチャンネル
  * Dチャンネルは、Hチャンネルに変換されます
  * Eチャンネルは、Iチャンネルに変換されます
  * Fチャンネルは、Jチャンネルに変換されます
 
<br>

# MMLコマンド
[MUCOM88](https://onitama.tv/mucom88/) に似せたコマンドになっています。  
~~~
音符
    音階      c d e f g a b （直後に、+を付けると♯、-を付けると♭）
    休符      r

コマンド
    音階      > < K k o D
    音量      ) ( V v
    音長      C l q . & ^ %
    リピート  [ ] /
    テンポ    t T
    その他    @ * ; : ! |

pコマンド
    pM 出力なし
    pL 左出力
    pR 右出力
    pC 中央出力

yコマンド
    yFB,パラメータ
    yAR1,パラメータ
    yDR1,パラメータ
    ySR1,パラメータ
    yRR1,パラメータ
    ySL1,パラメータ
    yTL1,パラメータ
    yKS1,パラメータ
    ySE1,パラメータ
    yAR2,パラメータ
    yDR2,パラメータ
    ySR2,パラメータ
    yRR2,パラメータ
    ySL2,パラメータ
    yTL2,パラメータ
    yKS2,パラメータ
    ySE2,パラメータ
    パラメータの頭文字に $ を付けると、16進数で指定できます。
~~~

* 備考
  * FBは、主トラックに掛かり、副トラックには掛かりません。
  * Dコマンドとpコマンドは、主トラックと副トラックの両方に掛かります。
  * Lコマンドは、主トラックの指定が有効となり、副トラックは強制的にループが適用されます。
  * tコマンドとTコマンドは、全トラックに掛かります。
