import React, { Fragment, useState } from "react";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";
import { useGridSlotComponentProps } from "@material-ui/data-grid";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  makeStyles,
  TextField,
} from "@material-ui/core";
import UserService from "services/user";

const useStyles = makeStyles((theme) => ({
  textField: {
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
    width: "100%",
    height: 90,
  },
}));

const today = (minutes = 0) => {
  const dt = new Date(new Date().getTime() + minutes * 60000);
  const Y = String(dt.getFullYear()).padStart(4, "0");
  const M = String(dt.getMonth() + 1).padStart(2, "0");
  const D = String(dt.getDate()).padStart(2, "0");
  const H = String(dt.getHours()).padStart(2, "0");
  const m = String(dt.getMinutes()).padStart(2, "0");
  return `${Y}-${M}-${D}T${H}:${m}`;
};

const GridToolbarScheduler = (props) => {
  const classes = useStyles();

  const [open, setOpen] = useState(false);
  const [start, setStart] = useState(today());
  const [stop, setStop] = useState(today(60));

  const grid = useGridSlotComponentProps();
  const selected = grid.apiRef.current.getSelectedRows();
  const [id] = Array.from(selected.keys());
  const data = selected.get(id);

  const schedule = () => {
    UserService.schedule(id, start.concat(":00"), stop.concat(":00"))
      .then(() => setOpen(false))
      .catch((e) => alert("Something wrong!!"));
  };

  return (
    <Fragment>
      {selected.size > 0 && (
        <Button color="primary" onClick={() => setOpen(true)}>
          Schedule
        </Button>
      )}

      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">
          {data?.sName?.toUpperCase()} ({id})
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please select start and stop time to activate user.
          </DialogContentText>
          <form noValidate>
            <Grid container spacing={1}>
              <Grid container item xs={12} spacing={3}>
                <TextField
                  label="Start activation"
                  type="datetime-local"
                  value={start}
                  onChange={(e) => setStart(e.target.value)}
                  className={classes.textField}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              </Grid>
              <Grid container item xs={12} spacing={3}>
                <TextField
                  label="Stop activation"
                  type="datetime-local"
                  value={stop}
                  onChange={(e) => setStop(e.target.value)}
                  className={classes.textField}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              </Grid>
            </Grid>
          </form>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)} color="primary">
            Cancel
          </Button>
          <Button onClick={() => schedule()} color="primary">
            Schedule
          </Button>
        </DialogActions>
      </Dialog>
    </Fragment>
  );
};

export default GridToolbarScheduler;
