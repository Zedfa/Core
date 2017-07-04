using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Rep.DTO;

namespace Core.Service
{
    public interface IViewElementService : IServiceBase<ViewElement>
    {
        ViewElement GetViewElementAndViewElementChildByID(int viewElemntId);
        IQueryable<ViewElement> GetRootViewElements();
        IList<ViewElement> GetViewElement(int? id);
        IQueryable<ViewElement> GetChildViewElementByParentId(int? parentId);
        //IList<ViewElement> GetAllViewElement(int? id);
        //int Create(List<int> inseretedviewElementRole, List<int> deletedviewElementRole, int OrganizationChartId, bool allowSaveChange = true);
        bool RoleHasAccess(int userId, string uniqueName, string urlParam = "");
        bool HasAnonymousAccess(string requestedUrl);
        MenuItem AccessibleViewElements(int userId);
        //void SetViewElementGrantedToUser(string userName);
        bool IsDuplicateUniqueName(string uniqueName, int viewElementId);
        int Delete(int Id, string userName);
        ViewElement GetViewElementAndChildsById(int id);
        ViewElement GetViewElementByUniqueName(string uniqueName);
        List<ViewElementDTO> GetAccessibleViewElements(int userId);
        int GetNewViewElementId();

    }
}
