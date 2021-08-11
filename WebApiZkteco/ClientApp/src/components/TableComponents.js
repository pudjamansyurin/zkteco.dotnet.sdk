import { makeStyles } from "@material-ui/core";
import LinearProgress from "@material-ui/core/LinearProgress";
import {
  GridOverlay,
  GridToolbarContainer,
  GridToolbarColumnsButton,
  GridToolbarFilterButton,
  GridToolbarExport,
  GridToolbarDensitySelector,
} from "@material-ui/data-grid";
import GridToolbarScheduler from "pages/user/GridToolbarScheduler";

const useStyles = makeStyles((theme) => ({
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

const TableComponents = {
  Toolbar: CustomToolbar,
  LoadingOverlay: CustomLoadingOverlay,
};

export default TableComponents;
