import React, { useEffect, useState } from "react";
import clsx from "clsx";
import { makeStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import LinearProgress from "@material-ui/core/LinearProgress";
import {
  DataGrid,
  GridOverlay,
  GridToolbarContainer,
  GridToolbarColumnsButton,
  GridToolbarFilterButton,
  GridToolbarExport,
  GridToolbarDensitySelector,
} from "@material-ui/data-grid";
import GridToolbarScheduler from "./GridToolbarScheduler";
import api from "utils/api";
import { columns } from "config/user";

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
  loading: { position: "absolute", top: 0, width: "100%" },
}));

function CustomToolbar() {
  return (
    <GridToolbarContainer>
      <GridToolbarColumnsButton />
      <GridToolbarFilterButton />
      <GridToolbarDensitySelector />
      <GridToolbarExport />
      <GridToolbarScheduler />
    </GridToolbarContainer>
  );
}

function CustomLoadingOverlay() {
  const classes = useStyles();

  return (
    <GridOverlay>
      <div className={classes.loading}>
        <LinearProgress />
      </div>
    </GridOverlay>
  );
}

export const UserPage = () => {
  const classes = useStyles();
  const [users, setUsers] = useState([]);
  const [selectedUsers, setSelectedUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);

  useEffect(() => {
    api
      .get("user")
      .then((data) => setUsers(data))
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  return (
    <Grid container spacing={3}>
      {/* <Grid item xs={12} md={8} lg={9}> */}
      <Grid item xs={12}>
        <Paper className={fixedHeightPaper}>
          <DataGrid
            rows={users}
            columns={columns}
            getRowId={(row) => row.sUserID}
            components={{
              Toolbar: CustomToolbar,
              LoadingOverlay: CustomLoadingOverlay,
            }}
            onSelectionModelChange={(newSelectionModel) => {
              setSelectedUsers(newSelectionModel);
            }}
            selectionModel={selectedUsers}
            loading={loading}
            disableMultipleSelection
            // checkboxSelection
            // disableSelectionOnClick
          />
        </Paper>
      </Grid>
      {/* <Grid item xs={12} md={4} lg={3}>
        <Paper className={fixedHeightPaper}></Paper>
      </Grid> */}
      {/* <Grid item xs={12}>
        <Paper className={classes.paper}>
        </Paper>
      </Grid> */}
    </Grid>
  );
};
