DECLARE @sql NVARCHAR(MAX)
            DECLARE statCursor CURSOR FOR 
            SELECT 
                'DROP STATISTICS ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) 
                                    + '.' + QUOTENAME(t.name) 
                                    + '.' + QUOTENAME(st.name) AS sql
            FROM
                sys.stats AS st 
                INNER JOIN sys.tables AS t
                    ON st.object_id = t.object_id
            WHERE
                st.user_created = 1 AND
                t.name = '{0}' AND
                st.name = '{1}'
            ORDER BY 1;

            OPEN statCursor;
             FETCH NEXT FROM statCursor INTO @sql
            WHILE @@FETCH_STATUS = 0  
            BEGIN  
                PRINT @sql
                EXEC sp_executesql @sql
                FETCH NEXT FROM statCursor INTO @sql
            END  
            CLOSE statCursor  
            DEALLOCATE statCursor