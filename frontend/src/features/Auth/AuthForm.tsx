import * as React from "react";
import { useEffect } from "react";
import Button from "@mui/material/Button";
import CssBaseline from "@mui/material/CssBaseline";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import store from "./store";

const defaultTheme = createTheme();

function Copyright(props: any) {
  return (
    <Typography variant="body2" color="text.secondary" align="center" {...props}>
      {"Copyright © Бишкекглавархитектура "} {new Date().getFullYear()}
      {"."}
    </Typography>
  );
}

const AuthForm = observer(() => {
  const { t: translate } = useTranslation();
  const navigate = useNavigate();

  useEffect(() => {
    store.setNavigateFunction(navigate);
  }, [navigate]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    store.onLogin();
  };

  return (
    <ThemeProvider theme={defaultTheme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 8,
            display: "flex",
            flexDirection: "column",
            alignItems: "center"
          }}
        >
          <img src={`${process.env.PUBLIC_URL}/logo256.png`} alt="Logo" width="50%" />
          <Typography component="h1" variant="h3" sx={{ mt: 2 }}>
            Бишкекглавархитектура
          </Typography>
          <Typography component="h1" variant="h5" sx={{ mt: 2 }}>
            {translate("label:authorization:signIn")}
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="pin"
              label="PIN-код USB-токена"
              name="pin"
              type="password"
              autoComplete="current-password"
              value={store.pin}
              onChange={(event: React.ChangeEvent<HTMLInputElement>) => store.handleChange(event)}
              error={!!store.error}
              helperText={store.error}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              {translate("label:authorization:signIn")}
            </Button>
          </Box>
        </Box>
        <Copyright sx={{ mt: 8, mb: 4 }} />
      </Container>
    </ThemeProvider>
  );
});

export default AuthForm;