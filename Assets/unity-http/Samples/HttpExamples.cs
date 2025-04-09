using System;
using System.Threading.Tasks;
using UnityHttp;
using UnityEngine;

public class HttpExamples : MonoBehaviour
{
    [ContextMenu(nameof(SimpleGet))]
    private async Task SimpleGet()
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

    [ContextMenu(nameof(ReuseRequest))]
    private async Task ReuseRequest()
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
            Debug.LogError(e.Response.StatusCode);
        }
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