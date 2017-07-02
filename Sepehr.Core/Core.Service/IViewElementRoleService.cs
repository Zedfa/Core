using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;
using Core.Service.Models;

namespace Core.Service
{
    public interface IViewElementRoleService : IServiceBase<ViewElementRole>
    {
        int Create(List<int> inseretedviewElementRole, List<int> deletedviewElementRole, int roleId, bool allowSaveChange = true);
        int UpdateByRoleId(
          int roleId,
          List<int> insertedViewElement,
          List<int> removedViewElement,
          bool allowSaveChange = true
          );
        void SetViewElementGrantedToUser(UserProfile userProfile);
        IList<ViewElementInfo> GetViewElementGrantedToUser(int userId);
        IQueryable<ViewElement> GetRootViewElementsBasedOnCompany(int? id);
        List<int> GetViewElementsIdByRoleId(int? roleId);
        IQueryable<ViewElement> GetViewElementsByRoleId(int? roleId);

    }
}
