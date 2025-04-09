using Duck.Http;
using Duck.Http.Service;
using UnityEngine;

public class HttpExamples : MonoBehaviour
{
    [ContextMenu(nameof(SimpleGet))]
    private async void SimpleGet()
    {
        try
        {
            var response = await Http.Get("https://jsonplaceholder.typicode.com/todosa/1").SendAsync();
            Debug.Log(response.Text);
        }
        catch (ProtocolException e)
        {
            Debug.LogError(e.Error.StatusCode);
        }
        catch (ConnectionException e)
        {
            Debug.LogError(e.Error.Text);
        }
    }

    [ContextMenu(nameof(ReuseRequest))]
    private async void ReuseRequest()
    {
        var request = Http.Get("https://jsonplaceholder.typicode.com/todos/1");
        try
        {
            var response = await request.SendAsync();
            Debug.Log(response.Text);
            
            var response2 = await request.SendAsync();
            Debug.Log(response2.Text);
        }
        catch (ProtocolException e)
        {
            Debug.LogError(e.Error.StatusCode);
        }
        catch (ConnectionException e)
        {
            Debug.LogError(e.Error.Text);
        }   
    }
}