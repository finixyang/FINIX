﻿using Finix.Code;
using Finix.Data;
using Finix.Data.Extensions;
using Finix.Domain.Entity.SystemSecurity;
using Finix.Domain.IRepository.SystemSecurity;

namespace Finix.Repository.SystemSecurity
{
    public class DbBackupRepository : RepositoryBase<DbBackupEntity>, IDbBackupRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var dbBackupEntity = db.FindEntity<DbBackupEntity>(keyValue);
                if (dbBackupEntity != null)
                {
                    FileHelper.DeleteFile(dbBackupEntity.F_FilePath);
                }
                db.Delete<DbBackupEntity>(dbBackupEntity);
                db.Commit();
            }
        }
        public void ExecuteDbBackup(DbBackupEntity dbBackupEntity)
        {
            DbHelper.ExecuteSqlCommand(string.Format("backup database {0} to disk ='{1}'", dbBackupEntity.F_DbName, dbBackupEntity.F_FilePath));
            dbBackupEntity.F_FileSize = FileHelper.ToFileSize(FileHelper.GetFileSize(dbBackupEntity.F_FilePath));
            dbBackupEntity.F_FilePath = "/Resource/DbBackup/" + dbBackupEntity.F_FileName;
            this.Insert(dbBackupEntity);
        }
    }
}