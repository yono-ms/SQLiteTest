# SQLiteTest
Xamarin Forms on SQLite.

| Platform | Build |
|:----|:----|
| iOS | [![Build status](https://build.appcenter.ms/v0.1/apps/dad9fbdd-1ed6-43c3-924e-99727bd24a05/branches/master/badge)](https://appcenter.ms) |
| Android | [![Build status](https://build.appcenter.ms/v0.1/apps/74e0e15b-fd22-49fb-96c8-06e56c9617bd/branches/master/badge)](https://appcenter.ms) |
| UWP | [![Build status](https://build.appcenter.ms/v0.1/apps/9ea3039a-ab0f-4092-83c5-d736903f910b/branches/master/badge)](https://appcenter.ms) |

## Overview
SQLiteを使わなければいけない場合の注意点を調べる。

## Package

https://docs.microsoft.com/ja-jp/xamarin/xamarin-forms/app-fundamentals/databases

sqlite-net-pcl

## SQL

MSサンプルを見ると、

- Where
- FirstOrDefaultAsync

を使えそうだが、実際には使えるコマンドは少ない。
Whereはプライマリキーの一致に使える。
.Equaulsを持つようなフィルターは使えない。
しかもコンパイル時には使えないことがわからず、
実行時に例外が発生する。

データ全部を取り出してLinQで絞るか、
文字列のSQL文をQueryAsyncで実行するかの二択になる。
文字列を使う場合パラメータ設定ツールは無いので、
自分でエスケープ処理を書く必要があるらしい。

MSのドキュメントから読み取れるのは以下の使い方。

- 主キーを設定する
- 主キーで検索及び削除を行う
- フィルタはLinQで

これでは小規模データにしか使えない。

## 警告

### warning MSB3277: "System.Numerics" の異なるバージョン間

オプションでMSBuildの出力を詳細にすると以下の情報が出てくる。

```
参照 "D:\VS\2019\Community\Common7\IDE\ReferenceAssemblies\Microsoft\Framework\Xamarin.iOS\v1.0\System.Numerics.dll" の原因となったプロジェクト ファイルの項目インクルード。

System.Numerics
C:\Microsoft\Xamarin\NuGet\xamarin.essentials\1.1.0\lib\xamarinios10\Xamarin.Essentials.dll

参照 "C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll" の原因となったプロジェクト ファイルの項目インクルード。

C:\Users\ono\.nuget\packages\sqlite-net-pcl\1.5.231\lib\netstandard1.1\SQLite-net.dll
C:\Users\ono\Source\Repos\SQLiteTest\SQLiteTest\SQLiteTest\bin\Debug\netstandard2.0\SQLiteTest.dll
```

XamarinEssentialsとSQLite.pclをiOSで同時に使用すると警告されるらしい。
この警告は解消することができない。

### mscorlibの間の競合を解決する方法がありません

"mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" と "mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e" の間の競合を解決する方法がありません。
プライマリであるため、"mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e" が選択されました。"mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" はプライマリではありませんでした。

こちらもSQLite.pclがV4.0.0.0を参照しているのが原因らしい。

PCLとか名前についている時点で駄目か。

## メリット

APIレスポンスをそのままDBにキャッシュする、
といった使い方をする場合定義を省略できる。
json文字列を保存する使い方でも検索キー以外を省略可能だが、
APIの項目がDBの項目としてそのまま使用できる。
（ただし上記の通りフィルターとしては使えない）

またCREATE TABLEもDBを使用したときに自動実行されるため、
SQL文不要となる。

よって従来3種類別に同じ定義をしていた、

- テーブル作成
- テーブル定義
- API定義

が1回で済む。
