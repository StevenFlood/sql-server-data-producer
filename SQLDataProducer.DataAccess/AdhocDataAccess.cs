﻿// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


namespace SQLDataProducer.DataAccess
{
    public class AdhocDataAccess : DataAccessBase
    {
        public AdhocDataAccess(string connectionString)
            : base (connectionString)
        {

        }

        public void ExecuteNonQuery(string query)
        {
            base.ExecuteNoResult(query);
        }

        public void TestConnection()
        {
            ExecuteNonQuery("select case when exists(select * from INFORMATION_SCHEMA.tables) then 1 else 0 end");
        }
    }
}
