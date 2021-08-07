import React, { useEffect } from "react";
import clsx from "clsx";
import { makeStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import { DataGrid } from "@material-ui/data-grid";
import api from "utils/api";

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: theme.spacing(2),
    display: "flex",
    overflow: "auto",
    flexDirection: "column",
  },
  fixedHeight: {
    height: 240,
  },
}));

const rows = [
  { id: 1, col1: "Hello", col2: "World" },
  { id: 2, col1: "XGrid", col2: "is Awesome" },
  { id: 3, col1: "Material-UI", col2: "is Amazing" },
];
const columns = [
  { field: "col1", headerName: "Column 1", width: 150 },
  { field: "col2", headerName: "Column 2", width: 150 },
];

export const UserPage = () => {
  const classes = useStyles();
  // const [user, setUser] = useState([]);
  const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);

  useEffect(() => {
    api
      .get("user")
      .then((res) => {
        console.info(res);
      })
      .catch((err) => console.error(err));
  }, []);

  return (
    <Grid container spacing={3}>
      {/* <Grid item xs={12} md={8} lg={9}> */}
      <Grid item xs={12}>
        <Paper className={fixedHeightPaper}>
          <DataGrid rows={rows} columns={columns} checkboxSelection />
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
