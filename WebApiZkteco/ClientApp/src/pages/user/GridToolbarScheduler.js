import React, { useState } from "react";
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

const useStyles = makeStyles((theme) => ({
  container: {
    display: "flex",
    flexWrap: "wrap",
  },
  textField: {
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
    width: 200,
  },
}));

const GridToolbarScheduler = (props) => {
  const classes = useStyles();

  const [open, setOpen] = useState(false);
  const grid = useGridSlotComponentProps();
  const selected = grid.apiRef.current.getSelectedRows();
  const [id] = Array.from(selected.keys());
  const data = selected.get(id);

  console.log(id, data);
  return (
    <span>
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
          Schedule {data?.sName?.toUpperCase()}
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please select start and stop time to activate user.
          </DialogContentText>
          <form className={classes.container} noValidate>
            <TextField
              id="datetime-local"
              label="Start activation"
              type="datetime-local"
              defaultValue="2017-05-24T10:30"
              className={classes.textField}
              InputLabelProps={{
                shrink: true,
              }}
            />
            <TextField
              id="datetime-local"
              label="Stop activation"
              type="datetime-local"
              defaultValue="2017-05-24T10:30"
              className={classes.textField}
              InputLabelProps={{
                shrink: true,
              }}
            />
          </form>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)} color="primary">
            Cancel
          </Button>
          <Button onClick={() => setOpen(false)} color="primary">
            Schedule
          </Button>
        </DialogActions>
      </Dialog>
    </span>
  );
};

export default GridToolbarScheduler;
