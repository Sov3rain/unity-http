using System;
using System.IO;
using System.Threading.Tasks;
using UnityHttp;
using UnityEngine;
using UnityHttp.Service;

public class HttpExamples : MonoBehaviour
{
    [ContextMenu(nameof(SimpleGet))]
    private void SimpleGet()
    {
        Http.Get("https://jsonplaceholder.typicode.com/todos/1")
            .OnSuccess(HandleSuccess)
            .Send();
    }

    private static void HandleSuccess(HttpResponse res)
    {
        var todo = JsonUtility.FromJson<Todo>(res.Text);
        Debug.Log(todo);
    }

    [ContextMenu(nameof(SimpleGetAsync))]
    private async Task SimpleGetAsync()
    {
        var response = await Http.Get("https://jsonplaceholder.typicode.com/todos/1").SendAsync();
        var todo = JsonUtility.FromJson<Todo>(response.Text);
        Debug.Log(todo);
    }

    [ContextMenu(nameof(SimpleGetThrows))]
    private async Task SimpleGetThrows()
    {
        try
        {
            var response = await Http.Get("https://jsonplaceholder.typicode.com/todoss/1").SendAsync();
            Debug.Log(response.Text);
        }
        catch (HttpException e)
        {
            Debug.LogError(e.Message);
        }
    }

    [ContextMenu(nameof(ReuseRequestThrows))]
    private async Task ReuseRequestThrows()
    {
        var request = Http.Get("https://jsonplaceholder.typicode.com/todos/1");
        try
        {
            var response = await request.SendAsync();
            Debug.Log(response.Text);

            var response2 = await request.SendAsync();
            Debug.Log(response2.Text);
        }
        catch (HttpException e)
        {
            Debug.LogError(e.Response.Error);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    [ContextMenu(nameof(GetFile))]
    private void GetFile()
    {
        string dir = Application.persistentDataPath;
        string filePath = Path.Combine(dir, "todo1.json");
        Http.GetFile("https://jsonplaceholder.typicode.com/todos/1", filePath).Send();
        Application.OpenURL(dir);
    }
}

[Serializable]
public class Todo
{
    public int userId;
    public int id;
    public string title;
    public bool completed;

    public override string ToString() => $"{title}: {completed}";
}