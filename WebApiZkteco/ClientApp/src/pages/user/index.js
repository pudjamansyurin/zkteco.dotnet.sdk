import React, { useEffect, useState } from "react";
import clsx from "clsx";
import { makeStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import { DataGrid } from "@material-ui/data-grid";
import { columns } from "config/user";
import UserService from "services/user";
import TableComponents from "components/TableComponents";

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: theme.spacing(2),
    display: "flex",
    overflow: "auto",
    flexDirection: "column",
  },
  fixedHeight: {
    height: 550,
    width: "100%",
  },
}));

export const UserPage = () => {
  const classes = useStyles();
  const [users, setUsers] = useState([]);
  const [selectedUsers, setSelectedUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);

  useEffect(() => {
    const interval = setInterval(() => {
      UserService.getAll()
        .then((data) => setUsers(data))
        .catch((err) => console.error(err))
        .finally(() => setLoading(false));
    }, 20 * 1000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  return (
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Paper className={fixedHeightPaper}>
          <DataGrid
            rows={users}
            columns={columns}
            getRowId={(row) => row.sUserID}
            components={TableComponents}
            onSelectionModelChange={(newSelectionModel) => {
              setSelectedUsers(newSelectionModel);
            }}
            selectionModel={selectedUsers}
            loading={loading}
            disableMultipleSelection
          />
        </Paper>
      </Grid>
    </Grid>
  );
};
