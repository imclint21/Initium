using Initium.Results;

namespace Initium.Services;

public interface ICrudService<TEntity>
{
	ServiceResult Create(TEntity entity);
	IEnumerable<TEntity> FindAll();
	IEnumerable<TEntity> FindById(Guid entityId);
	IEnumerable<TEntity> FindBy(Func<TEntity, bool> predicate);
	ServiceResult Update(Guid entityId, TEntity entity);
	ServiceResult Delete(Guid entityId);
	ServiceResult DeleteBy(Func<TEntity, bool> predicate);
	ServiceResult DeleteAll();
	
// 	BaseResult Create(TEntity entity);
// 	BaseResult Create(TCreateRequest createRequest);
// 	IQueryable<TEntity> Read();
// 	TEntity? Read(TPrimaryKeyType entityId);
// 	BaseResult Update(TPrimaryKeyType entityId, TEntity entity);
// 	BaseResult Delete(TPrimaryKeyType entityId);
// 	BaseResult Delete();
}