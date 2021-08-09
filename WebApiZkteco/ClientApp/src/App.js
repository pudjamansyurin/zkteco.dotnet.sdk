import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import CssBaseline from "@material-ui/core/CssBaseline";
import { UserPage } from "pages/user";
import TheAppBar from "components/TheAppBar";
import TheFooter from "components/TheFooter";
import TheDrawer from "components/TheDrawer";
import TheContainer from "components/TheContainer";
import TheMenuList from "components/TheMenuList";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
  },
}));

export default function App() {
  const classes = useStyles();
  const [open, setOpen] = useState(false);

  return (
    <div className={classes.root}>
      <CssBaseline />
      <TheAppBar open={open} onMenuOpen={() => setOpen(true)}></TheAppBar>
      <TheDrawer open={open} onMenuClose={() => setOpen(false)}>
        <TheMenuList></TheMenuList>
      </TheDrawer>

      <TheContainer>
        <UserPage></UserPage>
        <TheFooter></TheFooter>
      </TheContainer>
    </div>
  );
}
