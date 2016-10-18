# DynamicParameterHelper
This is a micro library adding support for dynamically creating a DynamicParameter collection for use with Dapper queries. 

# Usage
First you need a data model 
<pre>
    public class Person
    {
        [DynamicParameter(true, "@id", DbType.Int32)
        public int Id { get; set; }
        [DynamicParameter(false, "@description", DbType.AnsiString, Scalar = 140)
        public string Description { get; set; }
        [DynamicParameter(false, "@description", DbType.AnsiString, Scalar = 20)
        public string FirstName { get; set; }
    }
</pre>

and a stored procedure or query, I will demonstrate using a stored procedure, but this will work for parametarized queries as well
here are our two stored procedures that we will use our models to populate
<pre>
CREATE PROCEDURE dbo.InsertPerson(
    @description varchar(14o),
    @firstName varchar(20)
)
AS
BEGIN
    INSERT INTO dbo.Person (Description, FirstName)
    VALUES(@description, @firstName)
END
</pre>

<pre>
CREATE PROCEDURE dbo.UpdatePerson(
    @id int,
    @description varchar(50),
    @firstName varchar(20)
)
AS
BEGIN
    UPDATE dbo.Person
    SET Description = @description,
        FirstName = @firstName
    WHERE Id = @id
END
</pre>

To convert this into a stored procedure or dynamic query's parameters you need to do a little decoration to our Model

<pre>
    public class TestClass
    {
        [DynamicParameter(true, "@id", DbType.Int32)]
        public int Id { get; set; }
        [DynamicParameter(false, "@description", DbType.AnsiString, Scalar = 140)]
        public string Description { get; set; }
        [DynamicParameter(false, "@firstName", DbType.AnsiString, Scalar = 20)]
        public string FirstName { get; set; }
    }
</pre>

The DynamicParameter attribute has 3 required parameters 
<ul>
  <li>IsIdentity: does this property represent the identity column of the table you are upserting into?</li>
  <li>ParameterName: The Stored Procedure Parameter name</li>
  <li>Type: The DB type of the parameter</li>
</ul>
and 1 optional paramter
<ul>
<li>Scalar: the size of the paramter</li>
</ul>

then to populate your dynamic parameter object you just use the extension method ToDynamicParameters().
ToDynamicParameters takes an enum argument of what type of CRUD operation your stored proc or query will be doing. .
This controls weather or not the identity property is mapped to a dynamicparameter.

<pre>
var person = new Person();
var params = person.ToDynamicParameters(CrudType.Create);
</pre>

then to use your new parameters in a query you simple do

<pre>
  SqlConn.Query("dbo.InsertPerson", params, commandType: CommandType.StoredProcedure);
</pre>


