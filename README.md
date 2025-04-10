# Unity Http

A powerful and easy to use HTTP system for Unity. Cut down on the boilerplate code and get the most out of your HTTP requests.

- [Unity Http](#unity-http)
  - [Features](#features)
  - [Requirements](#requirements)
  - [Installation](#installation)
  - [Basic Usage](#basic-usage)
  - [Why Unity Http?](#why-unity-http)
  - [Examples](#examples)
  - [API](#api)
    - [Http Static Methods](#http-static-methods)
        - [Get](#get)
        - [Post](#post)
        - [Post JSON](#post-json)
        - [Put](#put)
        - [Delete](#delete)
        - [Misc](#misc)
    - [Http Request Configuration Methods](#http-request-configuration-methods)
        - [Headers](#headers)
        - [Events](#events)
        - [Progress](#progress)
        - [Configure](#configure)
    - [Http Request](#http-request)
    - [Http Response](#http-response)
        - [Properties](#properties)
    - [Super Headers](#super-headers)




## Features

* Async/Await support
* Singleton
* Fluent API for configuration
* Success, error and network error events
* Super headers

## Requirements

* Unity 2022.3+

## Installation

Add it as a package using [Unity Package Manager](https://docs.unity3d.com/Manual/upm-git.html) :

https://github.com/Sov3rain/unity-http.git?path=/Assets/unity-http

## Basic Usage

If you are using an AssemblyDefinition then reference the Http Assembly.  
Import the namespace `using UnityHttp;`


```c#
Http.Get("http://mywebapi.com/")
    .SetHeader("Authorization", "username:password")
    .OnSuccess(response => Debug.Log(response.Text))
    .OnError(response => Debug.Log(response.StatusCode))
    .OnDownloadProgress(progress => Debug.Log(progress))
    .Send();
```

Prefer async/await? We got you covered!

```c#
try
{
    var response = await Http.Get("http://mywebapi.com/")
        .SetHeader("Authorization", "username:password")
        .OnDownloadProgress(progress => Debug.Log(progress))
        .SendAsync();

    Debug.Log(response.Text);
}
catch (HttpException e)
{
    Debug.Log(e.Response.StatusCode);
}
```

## Why Unity Http?

Using UnityWebRequests can be a pain. A lot of boilerplate code, no async/await support, no return value and they must be run in a coroutine bound to a GameObject.

A simple request using UnityWebRequests looks like this:

```c#
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Get());
    }

    private IEnumerator Get()
    {
        var request = UnityWebRequest.Get("http://mywebapi.com/");  
        request.SetRequestHeader("Authorization", "username:password");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }
}
```

Using UnityHttp, you can do this:

```c#
using UnityEngine;
using UnityHttp;

public class Example : MonoBehaviour
{
    void Start()
    {
        Http.Get("http://mywebapi.com/")
            .SetHeader("Authorization", "username:password")
            .OnSuccess(response => Debug.Log(response.Text))
            .OnError(response => Debug.Log(response.StatusCode))
            .Send();
    }
}
```           
Note that requests does not need to be run in a coroutine bound to a GameObject.

## Examples

In this given example, the `response.Text` from `http://mywebapi.com/user.json` is:

```json
{
    "id": 92,
    "username": "jason"
}
```

Create a serializable class that maps the data from the json response to fields

```c#
[Serializable]
public class User
{
    [SerializeField]
    public int id;
    [SerializeField]
    public string username;
}
```

We can listen for the event `OnSuccess` with our handler method `HandleSuccess`

```c#
var request = Http.Get("http://mywebapi.com/user.json")
    .OnSuccess(HandleSuccess)
    .OnError(response => Debug.Log(response.StatusCode))
    .Send();
```

Parse the `response.Text` to the serialized class `User` that we declared earlier by using Unity's built in [JSONUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html)

```c#
private void HandleSuccess(HttpResponse response)
{
     var user = JsonUtility.FromJson<User>(response.Text);
     Debug.Log(user.username);
}
```

## API

### Http Static Methods

All these methods return a new HttpRequest.  

##### Get

* `Http.Get(string uri)`  
* `Http.GetTexture(string uri)`
* `Http.GetFile(string uri, string filePath)`
* `Http.GetAssetBundle(string uri)`
* `Http.GetAudioClip(string uri, AudioType audioType)`

##### Post

* `Http.Post(string uri, string postData)`  
* `Http.Post(string uri, WWWForm formData)`  
* `Http.Post(string uri, Dictionary<string, string> formData))`  
* `Http.Post(string uri, List<IMultipartFormSection> multipartForm)`  
* `Http.Post(string uri, byte[] bytes, string contentType)`  

##### Post JSON

* `Http.PostJson(string uri, string json)`

##### Put

* `Http.Put(string uri, byte[] bodyData)` 
* `Http.Put(string uri, string bodyData)` 

##### Delete

* `Http.Delete(string uri)`  

##### Misc

* `Http.Head(string uri)`  

### Http Request Configuration Methods

All these methods return the HttpRequest instance.  

##### Headers

* `SetHeader(string key, string value)`  
* `SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)`  
* `RemoveHeader(string key)`  
* `RemoveSuperHeaders()`

##### Events

* `OnSuccess(Action<HttpResonse> response)`  
* `OnError(Action<HttpResonse> response)`  
* `OnNetworkError(Action<HttpResonse> response)`  

##### Progress

* `OnUploadProgress(Action<float> progress)`  
* `OnDownloadProgress(Action<float> progress)`  

##### Configure

* `SetRedirectLimit(int redirectLimit)`   
* `SetTimeout(int duration)`

Redirect limit subject to Unity's documentation.  

* [Redirect Limit Documentation](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-redirectLimit.html)

Progress events will invoke each time the progress value has increased, they are subject to Unity's documentation.

* [Upload Progress Documentation](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-uploadProgress.html)
* [Download Progress Documentation](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest-downloadProgress.html)

### Http Request

* `HttpRequest Send()`  
* `void Abort()`  

### Http Response

The callbacks for `OnSuccess`, `OnError` and `OnNetworkError` all return you a `HttpResponse`.  
This has the following properties:  

##### Properties

* `string Url`  
* `bool IsSuccessful`  
* `bool IsHttpError`  
* `bool IsNetworkError`  
* `long StatusCode`  
* `ResponseType ResponseType`  
* `byte[] Bytes`  
* `string Text`  
* `string Error`  
* `Texture Texture`  
* `Dictionary<string, string> ResponseHeaders`  

### Super Headers

Super Headers are a type of Header that you can set once to automatically attach to every Request you’re sending.  
They are Headers that apply to all requests without having to manually include them in each HttpRequest SetHeader call.

* `Http.SetSuperHeader(string key, string value)`
* `Http.RemoveSuperHeader(string key)` returns `bool`
* `Http.GetSuperHeaders()` returns `Dictionary<string, string>`

