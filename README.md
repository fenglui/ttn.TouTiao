# toutiao

今日头条 dotnet core api

## Usage

- [toutiao](#toutiao)
  - [Usage](#usage)
  - [Features](#features)
  - [Prerequisites:](#prerequisites)
  - [Installation:](#installation)
  - [Getting Started:](#getting-started)

## Features

- **ASP.NET Core**
  - Web API

## Prerequisites:
 * [.Net Core SDK](https://www.microsoft.com/net/download/windows)
 * [TouTiao Develper](https://developer.toutiao.com/)

## Installation:
 * Install-Package ttn.TouTiao

## Getting Started:

 * AccessToken
  ```csharp
string _appId = "yr appid";
string _appSecret = "yr appSecret";
var task = AppsApi.GetAccessTokenAsync(_appId, _appSecret);
Assert.IsNotNull(task.Result);
Assert.IsNotNull(task.Result.Response.access_token);
Assert.IsTrue(task.Result.Response.expires_in > 0);
  ```
 * 
 * 