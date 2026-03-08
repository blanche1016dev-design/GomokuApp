# 五目並べ

C# と WinForms で作成した、Windows 向けの五目並べアプリです。  
`GomokuApp.exe` を実行して、CPU と対戦できます。

## 特徴

- Windows デスクトップアプリ
- 難易度 3 段階
  - Easy
  - Normal
  - Hard
- 先手 / 後手を選択可能
- Easy では危険局面で推奨手を表示

## 動作環境

- Windows 10 / 11
- 64bit 環境

## インストール方法

ユーザー向けの詳細手順は [インストールマニュアル.md](/c:/Users/katsu/OneDrive/デスクトップ/C%23/五目並べ/インストールマニュアル.md) を参照してください。

### 配布済み exe を使う場合

1. GitHub の `Releases` から配布ファイルをダウンロード
2. zip の場合は展開
3. `GomokuApp.exe` を実行

## 使い方

1. アプリを起動
2. 難易度を選択
3. 先手 / 後手を選択
4. `新しい対局を開始` をクリック
5. 盤面をクリックして石を置く

## 開発環境

- C#
- .NET 9
- WinForms
- xUnit

## ソースから実行

```powershell
dotnet run --project .\GomokuApp\GomokuApp.csproj
```

## ソースからビルド

```powershell
dotnet build .\GomokuApp.sln
```

## exe の出力

```powershell
dotnet publish .\GomokuApp\GomokuApp.csproj -c Release -r win-x64 --self-contained false
```

出力先:

```text
.\GomokuApp\bin\Release\net9.0-windows\win-x64\publish\
```

## テスト

```powershell
dotnet test .\GomokuApp.Tests\GomokuApp.Tests.csproj --no-restore
```

## ドキュメント

- 設計書: [設計書.md](/c:/Users/katsu/OneDrive/デスクトップ/C%23/五目並べ/設計書.md)
- インストール手順: [インストールマニュアル.md](/c:/Users/katsu/OneDrive/デスクトップ/C%23/五目並べ/インストールマニュアル.md)

## プロジェクト構成

- [GomokuApp](/c:/Users/katsu/OneDrive/デスクトップ/C%23/五目並べ/GomokuApp)
  - アプリ本体
- [GomokuApp.Tests](/c:/Users/katsu/OneDrive/デスクトップ/C%23/五目並べ/GomokuApp.Tests)
  - 単体テスト

## ライセンス

必要に応じて追記してください。
