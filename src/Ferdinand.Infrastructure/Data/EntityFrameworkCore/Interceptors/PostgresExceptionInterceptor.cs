// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;
// using Npgsql;
//
// namespace Ferdinand.Infrastructure.Data.EntityFrameworkCore.Interceptors;
//
// public class PostgresExceptionInterceptor : SaveChangesInterceptor
// {
//     public override void SaveChangesFailed(DbContextErrorEventData eventData)
//     {
//         var dbUpdateException = eventData.Exception as DbUpdateException;
//
//         if (eventData.Exception.GetBaseException() is PostgresException providerException)
//         {
//             var error = GetDatabaseError(providerException);
//
//             if (error != null && dbUpdateException != null)
//             {
//                 var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries);
//                 throw exception;
//             }
//         }
//
//         base.SaveChangesFailed(eventData);
//     }
//
//     public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
//         CancellationToken cancellationToken = new CancellationToken())
//     {
//         return base.SaveChangesFailedAsync(eventData, cancellationToken);
//     }
//     
//     private static string GetDatabaseError(PostgresException dbException)
//     {
//         return dbException.SqlState switch
//         {
//             PostgresErrorCodes.StringDataRightTruncation => DatabaseError.MaxLength,
//             PostgresErrorCodes.NumericValueOutOfRange => DatabaseError.NumericOverflow,
//             PostgresErrorCodes.NotNullViolation => DatabaseError.CannotInsertNull,
//             PostgresErrorCodes.UniqueViolation => DatabaseError.UniqueConstraint,
//             PostgresErrorCodes.ForeignKeyViolation => DatabaseError.ReferenceConstraint,
//             _ => null
//         };
//     }
// }
