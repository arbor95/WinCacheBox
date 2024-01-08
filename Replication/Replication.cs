using System;
using System.Data;
using System.Data.Common;
using WinCachebox.Geocaching;



namespace WinCachebox
{
    // for Replication between WinCB and CB
    public class Replication
    {
        

        public long Id = -1;
        public long CacheId = -1;
        public ChangeTypeEnum ChangeType = ChangeTypeEnum.Undefined;
        public string WpGcCode = "";
        public int SolverCheckSum = 0;
        public int NotesCheckSum = 0;
        public int WpCoordCheckSum = 0;

        public Replication(DbDataReader reader)
        {
            Id = long.Parse(reader[0].ToString());
            ChangeType = (ChangeTypeEnum)int.Parse(reader[1].ToString());
            CacheId = long.Parse(reader[2].ToString());
            if (reader[3].ToString() != null)
                WpGcCode = reader[3].ToString();
            if (reader[4].ToString() != "")
                SolverCheckSum = int.Parse(reader[4].ToString());
            if (reader[5].ToString() != "")
                NotesCheckSum = int.Parse(reader[5].ToString());
            if (reader[6].ToString() != "")
                WpCoordCheckSum = int.Parse(reader[6].ToString());
        }

        public bool Import(Database database)
        {
            switch (ChangeType)
            {
                case ChangeTypeEnum.SolverText:
                    return importSolverText(database);
                case ChangeTypeEnum.NotesText:
                    return importNotesText(database);
                case ChangeTypeEnum.WaypointChanged:
                    return importWaypointChanged(database);
                case ChangeTypeEnum.NewWaypoint:
                    return importWaypointChanged(database);
                case ChangeTypeEnum.DeleteWaypoint:
                    return importWaypointDeleted(database);
                case ChangeTypeEnum.Found:
                    return importFound(database, true);
                case ChangeTypeEnum.NotFound:
                    return importFound(database, false);
                case ChangeTypeEnum.NumTravelbugs:
                    return importNumTravelbugs(database);
                case ChangeTypeEnum.Available:
                    return importAvailable(database, true);
                case ChangeTypeEnum.NotAvailabe:
                    return importAvailable(database, false);
                case ChangeTypeEnum.Archived:
                    return importArchived(database, true);
                case ChangeTypeEnum.NotArchived:
                    return importArchived(database, false);
            }
                    return false;
            
        }

        private bool importSolverText(Database database)
        {
            // read Solver text form the actual database
            string aktSolverText;
            CBCommand acommand = Database.Data.CreateCommand("select Solver from Caches where Id=@id");
            acommand.ParametersAdd("@id", DbType.Int64, CacheId);
            try
            {
                aktSolverText = acommand.ExecuteScalar().ToString();
            }
            catch (Exception) { aktSolverText = ""; }
            acommand.Dispose();

            // read Solver text from Import database
            CBCommand command = database.CreateCommand("select Solver from Caches where Id=@id");
            command.ParametersAdd("@id", DbType.Int64, CacheId);
            string solverText;
            try
            {
                solverText = command.ExecuteScalar().ToString();
            }
            catch (Exception) { solverText = ""; }
            command.Dispose();

            if (database.serverType == Database.SqlServerType.SQLite)
            {
                // Zeilenwechsel von Notes von Android müssen an das System von Windows angepasst werden
                solverText = solverText.Replace("\n", Environment.NewLine);
            }

            if (solverText == aktSolverText)
                return true;  // nothing must be changed, Replicationsentry must be deleted

            int aktCheckSum = (int)Global.sdbm(aktSolverText);
            if (aktCheckSum != this.SolverCheckSum)
            {
                // The solver text was changed in both databases! -> conflict!
                // load Cache Name for viewing in the dialog
                string cacheName;
                CBCommand ccommand = Database.Data.CreateCommand("select Name from Caches where Id=@id");
                ccommand.ParametersAdd("@id", DbType.Int64, CacheId);
                try
                {
                    cacheName = ccommand.ExecuteScalar().ToString();
                }
                catch (Exception) { return false; }
                ccommand.Dispose();

                ReplicationConflictForm.ReplicationConflictResult conflictResult = ReplicationConflictForm.ShowConflict(cacheName, ChangeType, aktSolverText, solverText, true);
                switch (conflictResult)
                {
                    case ReplicationConflictForm.ReplicationConflictResult.DoNotSolve:
                        return false;  // return, do not delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseOriginal:
                        return true;   // return and delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseCopy:
                        break;      // do not return, store the new value in the database
                }
            }

            CBCommand wcommand = Database.Data.CreateCommand("update Caches set Solver=@solver where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@solver", DbType.String, solverText);
            wcommand.ExecuteNonQuery();
            wcommand.Dispose();

            return true;
        }
        private bool importNotesText(Database database)
        {
            // read Note text form the actual database
            string aktNoteText;
            CBCommand acommand = Database.Data.CreateCommand("select Notes from Caches where Id=@id");
            acommand.ParametersAdd("@id", DbType.Int64, CacheId);
            try
            {
                aktNoteText = acommand.ExecuteScalar().ToString();
            }
            catch (Exception exc)
                {
                    exc.GetType(); //Warning vermeiden _ avoid warning
                    aktNoteText = ""; 
                }
            acommand.Dispose();

            // read Note text from Import database
            CBCommand command = database.CreateCommand("select Notes from Caches where Id=@id");
            command.ParametersAdd("@id", DbType.Int64, CacheId);
            string NoteText;
            try
            {
                NoteText = command.ExecuteScalar().ToString();
            }
            catch (Exception exc)
                {
                    exc.GetType(); //Warning vermeiden _ avoid warning
                    NoteText = "";
                }
            command.Dispose();

            if (database.serverType == Database.SqlServerType.SQLite)
            {
                // Zeilenwechsel von Notes von Android müssen an das System von Windows angepasst werden
                NoteText = NoteText.Replace("\n", Environment.NewLine);
            }


            if (NoteText == aktNoteText)
                return true;  // nothing must be changed, Replicationsentry must be deleted

            int aktCheckSum = (int)Global.sdbm(aktNoteText);
            if (aktCheckSum != this.NotesCheckSum)
            {
                // The Note text was changed in both databases! -> conflict!
                // load Cache Name for viewing in the dialog
                string cacheName;
                CBCommand ccommand = Database.Data.CreateCommand("select Name from Caches where Id=@id");
                ccommand.ParametersAdd("@id", DbType.Int64, CacheId);
                try
                {
                    cacheName = ccommand.ExecuteScalar().ToString();
                }
                catch (Exception) { return false; }
                ccommand.Dispose();

                ReplicationConflictForm.ReplicationConflictResult conflictResult = ReplicationConflictForm.ShowConflict(cacheName, ChangeType, aktNoteText, NoteText, true);
                switch (conflictResult)
                {
                    case ReplicationConflictForm.ReplicationConflictResult.DoNotSolve:
                        return false;  // return, do not delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseOriginal:
                        return true;   // return and delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseCopy:
                        break;      // do not return, store the new value in the database
                }
            }

            CBCommand wcommand = Database.Data.CreateCommand("update Caches set Notes=@notes where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@notes", DbType.String, NoteText);
            wcommand.ExecuteNonQuery();
            wcommand.Dispose();

            return true;
        }
        private bool importWaypointChanged(Database database)
        {
            // read Waypoint of WinCB database 
            CBCommand command = Database.Data.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint where GcCode=@gccode");
            command.ParametersAdd("@gccode", DbType.String, this.WpGcCode);
            DbDataReader reader = command.ExecuteReader();
            Waypoint waypoint = null;
            if (reader.Read())
                waypoint = new Waypoint(reader);
            reader.Dispose();
            command.Dispose();
            int checkSum = 0;
            if (waypoint != null)
                checkSum = waypoint.CreateCheckSum();

            // read Waypoint of CB database 
            command = database.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint where GcCode=@gccode");
            command.ParametersAdd("@gccode", DbType.String, this.WpGcCode);
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }


            Waypoint waypointImport = null;
            if (reader.Read())
                waypointImport = new Waypoint(reader);

            reader.Dispose();
            command.Dispose();

            if (waypointImport == null)
                return true;

            // compare waypoint checksum with replications-checksum
            if (checkSum != this.WpCoordCheckSum)
            {
                // the waypoint is changed in both databases!
                // load Cache Name for viewing in the dialog
                string cacheName;
                CBCommand ccommand = Database.Data.CreateCommand("select Name from Caches where Id=@id");
                ccommand.ParametersAdd("@id", DbType.Int64, CacheId);
                try
                {
                    cacheName = ccommand.ExecuteScalar().ToString();
                }
                catch (Exception) { return false; }
                ccommand.Dispose();


                string swaypoint = "";
                string swaypointImport = "";
                if (waypoint != null)
                    swaypoint = waypoint.CreateConflictString();
                swaypointImport = waypointImport.CreateConflictString();
                ReplicationConflictForm.ReplicationConflictResult conflictResult = ReplicationConflictForm.ShowConflict(cacheName, ChangeType, swaypoint, swaypointImport, true);
                switch (conflictResult)
                {
                    case ReplicationConflictForm.ReplicationConflictResult.DoNotSolve:
                        return false;  // return, do not delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseOriginal:
                        return true;   // return and delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseCopy:
                        break;      // do not return, store the new value in the database
                }
            }

            if (waypoint != null)
                waypointImport.UpdateDatabase();
            else
                waypointImport.WriteToDatabase();

            return true;
        }
        private bool importWaypointDeleted(Database database)
        {
            // read Waypoint of WinCB database 
            CBCommand command = Database.Data.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint where GcCode=@gccode");
            command.ParametersAdd("@gccode", DbType.String, this.WpGcCode);
            DbDataReader reader = command.ExecuteReader();
            Waypoint waypoint = null;
            if (reader.Read())
                waypoint = new Waypoint(reader);
            reader.Dispose();
            command.Dispose();
            int checkSum = 0;
            if (waypoint != null)
                checkSum = waypoint.CreateCheckSum();

            if (waypoint == null)
                return true;
            // compare waypoint checksum with replications-checksum
            if (checkSum != this.WpCoordCheckSum)
            {
                // the waypoint is changed in WinCB  databases and deleted in CB database -> conflict
                // load Cache Name for viewing in the dialog
                string cacheName;
                CBCommand ccommand = Database.Data.CreateCommand("select Name from Caches where Id=@id");
                ccommand.ParametersAdd("@id", DbType.Int64, CacheId);
                try
                {
                    cacheName = ccommand.ExecuteScalar().ToString();
                }
                catch (Exception) { return false; }
                ccommand.Dispose();


                string swaypoint = "";
                string swaypointImport = "deleted in CacheBox!";
                if (waypoint != null)
                    swaypoint = waypoint.CreateConflictString();

                ReplicationConflictForm.ReplicationConflictResult conflictResult = ReplicationConflictForm.ShowConflict(cacheName, ChangeType, swaypoint, swaypointImport, true);
                switch (conflictResult)
                {
                    case ReplicationConflictForm.ReplicationConflictResult.DoNotSolve:
                        return false;  // return, do not delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseOriginal:
                        return true;   // return and delete this replication entry in the import database
                    case ReplicationConflictForm.ReplicationConflictResult.UseCopy:
                        break;      // do not return, store the new value in the database
                }
            }
            // Delete Waypoint
            waypoint.DeleteFromDatabase();

            return true;
        }
        private bool importFound(Database database, bool found)
        {
            CBCommand wcommand = Database.Data.CreateCommand("update Caches set Found=@found where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@found", DbType.Boolean, found);
            int i = wcommand.ExecuteNonQuery();
            wcommand.Dispose();
            return true;
        }

        private bool importNumTravelbugs(Database database)
        {
            CBCommand wcommand = Database.Data.CreateCommand("update Caches set NumTravelbugs=@numtravelbugs where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@numtravelbugs", DbType.Int16, SolverCheckSum);
            int i = wcommand.ExecuteNonQuery();
            wcommand.Dispose();
            return true;
        }

        private bool importAvailable(Database database, bool available)
        {
            CBCommand wcommand = Database.Data.CreateCommand("update Caches set Available=@available where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@available", DbType.Boolean, available);
            int i = wcommand.ExecuteNonQuery();
            wcommand.Dispose();
            return true;
        }

        private bool importArchived(Database database, bool archived)
        {
            CBCommand wcommand = Database.Data.CreateCommand("update Caches set Archived=@archived where Id=@id");
            wcommand.ParametersAdd("@id", DbType.Int64, CacheId);
            wcommand.ParametersAdd("@archived", DbType.Boolean, archived);
            int i = wcommand.ExecuteNonQuery();
            wcommand.Dispose();
            return true;

        }
    }
}
