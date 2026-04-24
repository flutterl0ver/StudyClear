using DatabaseServerProgram.DTO;

namespace DatabaseServerProgram.Interfaces;

public interface IHttpController {
    public object HandleRequest(HttpRequest request);
}