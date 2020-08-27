using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PropAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Prop;

        public PropAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Create(Prop _data)
        {
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.Create() : Opening SQL connection");
                using (var conn = Connection)
                {
                    SimpleTransaction transaction = null;
                    try
                    {
                        bool result = false;
                        transaction = conn.BeginTransaction();
                        // PROCEDURE: CreateProp
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Prop, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Id, _data.id);

                            cmd.Set(Mabinogi.SQL.Columns.Prop.ClassId, _data.classid);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Region, _data.region);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.X, _data.x);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Y, _data.y);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Z, _data.z);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Direction, _data.direction);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Scale, _data.scale);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color1, _data.color1);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color2, _data.color2);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color3, _data.color3);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color4, _data.color4);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color5, _data.color5);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color6, _data.color6);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color7, _data.color7);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color8, _data.color8);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color9, _data.color9);

                            cmd.Set(Mabinogi.SQL.Columns.Prop.Name, _data.name);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.State, _data.state);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.EnterTime, _data.entertime);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Extra, _data.extra);

                            WorkSession.WriteStatus("PropSqlAdapter.Create() : Executing prop insert");
                            result = cmd.Execute() > 0;
                        }

                        if (_data.@event != null)
                        {
                            foreach (PropEvent propEvent in _data.@event)
                            {
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PropEvent, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Id, _data.id);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Default, propEvent.@default);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Signal, propEvent.signal);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Type, propEvent.type);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Extra, propEvent.extra);

                                    WorkSession.WriteStatus("PropSqlAdapter.Create() : Executing prop event insert");
                                    cmd.Execute();
                                }
                            }
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        transaction?.Rollback();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        transaction?.Rollback();
                        return false;
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.Create() : Leaving function");
            }
        }

        public bool Delete(long _id)
        {
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.Delete() : Opening SQL connection");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.Prop))
                {
                    // PROCEDURE: DeleteProp
                    cmd.Where(Mabinogi.SQL.Columns.Prop.Id, _id);

                    WorkSession.WriteStatus("PropSqlAdapter.Delete() : Executing command");
                    return cmd.Execute() > 0;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.Delete() : Leaving function");
            }
        }

        public Prop Read(long _id)
        {
            Prop result = null;
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.Read() : Opening SQL connection");
                using (var conn = Connection)
                {
                    // PROCEDURE: SelectProp
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Prop))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Prop.Id, _id);

                        WorkSession.WriteStatus("PropSqlAdapter.Read() : Executing prop reader");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                            {
                                WorkSession.WriteStatus(string.Format("PropSqlAdapter.Read() : Prop {0} does not exist", _id));
                                return null;
                            }

                            result = new Prop();
                            result.id = _id;
                            result.classid = reader.GetInt32(Mabinogi.SQL.Columns.Prop.ClassId);
                            result.x = reader.GetInt32(Mabinogi.SQL.Columns.Prop.X);
                            result.y = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Y);
                            result.z = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Z);
                            result.direction = reader.GetFloat(Mabinogi.SQL.Columns.Prop.Direction);
                            result.scale = reader.GetFloat(Mabinogi.SQL.Columns.Prop.Scale);
                            result.color1 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color1);
                            result.color2 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color2);
                            result.color3 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color3);
                            result.color4 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color4);
                            result.color5 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color5);
                            result.color6 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color6);
                            result.color7 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color7);
                            result.color8 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color8);
                            result.color9 = reader.GetInt32(Mabinogi.SQL.Columns.Prop.Color9);
                            result.name = reader.GetString(Mabinogi.SQL.Columns.Prop.Name);
                            result.state = reader.GetString(Mabinogi.SQL.Columns.Prop.State);
                            result.entertime = reader.GetInt64(Mabinogi.SQL.Columns.Prop.EnterTime);
                            result.extra = reader.GetString(Mabinogi.SQL.Columns.Prop.Extra);
                        }
                    }


                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PropEvent))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PropEvent.Id, _id);

                        WorkSession.WriteStatus("PropSqlAdapter.Read() : Executing prop event reader");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result.@event = new List<PropEvent>();
                                PropEvent propEvent;
                                while (reader.Read())
                                {
                                    propEvent = new PropEvent();
                                    propEvent.@default = reader.GetBoolean(Mabinogi.SQL.Columns.PropEvent.Default);
                                    propEvent.signal = reader.GetInt32(Mabinogi.SQL.Columns.PropEvent.Signal);
                                    propEvent.type = reader.GetInt32(Mabinogi.SQL.Columns.PropEvent.Type);
                                    propEvent.extra = reader.GetString(Mabinogi.SQL.Columns.PropEvent.Extra);

                                    result.@event.Add(propEvent);
                                }
                            }
                        }
                    }
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.Read() : Leaving function");
            }

            return result;
        }

        public bool Write(Prop _data)
        {
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.Write() : Opening SQL connection");
                using (var conn = Connection)
                {
                    SimpleTransaction transaction = null;
                    try
                    {
                        bool result = false;
                        transaction = conn.BeginTransaction();
                        // PROCEDURE: InsertProp
                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Prop, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Prop.Id, _data.id);

                            cmd.Set(Mabinogi.SQL.Columns.Prop.ClassId, _data.classid);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Region, _data.region);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.X, _data.x);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Y, _data.y);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Z, _data.z);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Direction, _data.direction);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Scale, _data.scale);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color1, _data.color1);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color2, _data.color2);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color3, _data.color3);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color4, _data.color4);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color5, _data.color5);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color6, _data.color6);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color7, _data.color7);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color8, _data.color8);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Color9, _data.color9);

                            cmd.Set(Mabinogi.SQL.Columns.Prop.Name, _data.name);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.State, _data.state);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.EnterTime, _data.entertime);
                            cmd.Set(Mabinogi.SQL.Columns.Prop.Extra, _data.extra);

                            WorkSession.WriteStatus("PropSqlAdapter.Write() : Executing prop update");
                            result = cmd.Execute() > 0;
                        }

                        using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PropEvent, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.PropEvent.Id, _data.id);

                            WorkSession.WriteStatus("PropSqlAdapter.Write() : Executing prop event delete");
                            cmd.Execute();
                        }

                        if (_data.@event != null)
                        {
                            foreach (PropEvent propEvent in _data.@event)
                            {
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PropEvent, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Id, _data.id);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Default, propEvent.@default);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Signal, propEvent.signal);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Type, propEvent.type);
                                    cmd.Set(Mabinogi.SQL.Columns.PropEvent.Extra, propEvent.extra);

                                    WorkSession.WriteStatus("PropSqlAdapter.Write() : Executing prop event insert");
                                    cmd.Execute();
                                }
                            }
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        transaction?.Rollback();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        transaction?.Rollback();
                        return false;
                    }
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.Write() : Leaving function");
            }
        }

        public PropIDList LoadPropList()
        {
            WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Prop))
                    {
                        WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 명령을 실행합니다");
                        cmd.Set(Mabinogi.SQL.Columns.Prop.Id, 0);

                        WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 쿼리 결과에서 프랍 리스트를 얻어옵니다");
                        using (var reader = cmd.ExecuteReader())
                        {

                            if (reader == null)
                            {
                                throw new Exception("쿼리된 데이터가 없습니다");
                            }

                            if (reader.HasRows == false)
                            {
                                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 쿼리 결과를 프랍 리스트로 변환할 수 없습니다");
                                return null;
                            }

                            List<long> propIDs = new List<long>();

                            while (reader.Read())
                            {
                                propIDs.Add(reader.GetInt64(Mabinogi.SQL.Columns.Prop.Id));
                            }

                            return new PropIDList() { propID = propIDs };
                        }
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 연결을 종료합니다");
            }
        }
    }
}
