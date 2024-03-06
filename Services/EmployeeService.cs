using Grpc.Core;
using GrpcService1.DataAccess;
using GrpcService1.Protos;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Services
{
    public class EmployeeService : EmployeeRpcService.EmployeeRpcServiceBase
    {
        private readonly AppDbContext db;

        public EmployeeService(AppDbContext db)
        {
            this.db = db;
        }

        public override async Task<EmployeeList> SelectAll(Empty request, ServerCallContext context)
        {
            var query = from e in db.Employees
                        select new EmployeeEntity()
                        {
                            EmployeeId = e.EmployeeId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                        };

            List<EmployeeEntity> employees = await query.ToListAsync();

            return new EmployeeList { Employees = { employees } };
        }

        public override async Task<EmployeeEntity?> SelectById(EmployeeFilter request, ServerCallContext context)
        {
            var query = from e in db.Employees
                        where e.EmployeeId == request.EmployeeId
                        select new EmployeeEntity()
                        {
                            EmployeeId = e.EmployeeId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                        };

            return await query.FirstOrDefaultAsync();
        }

        public override async Task<Empty> Insert(CreateEmployee request, ServerCallContext context)
        {
            Employee e = new Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var id = db.Employees.Max(e => e.EmployeeId) + 1;
            e.EmployeeId = id;

            db.Employees.Add(e);
            await db.SaveChangesAsync();
            return new Empty();
        }

        public override async Task<Empty> Update(UpdateEmployee request, ServerCallContext context)
        {
            var e = db.Employees.Find(request.EmployeeId);

            if (e == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
            }

            e.FirstName = request.FirstName;
            e.LastName = request.LastName;
            await db.SaveChangesAsync();
            return new Empty();
        }

        public override async Task<Empty> Delete(EmployeeFilter request, ServerCallContext context)
        {
            var e = db.Employees.Find(request.EmployeeId);

            if (e == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Employee not found"));
            }

            db.Employees.Remove(e);
            await db.SaveChangesAsync();
            return new Empty();
        }
    }
}
