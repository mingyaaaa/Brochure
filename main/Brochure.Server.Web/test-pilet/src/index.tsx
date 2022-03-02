import * as React from 'react';
import { PiletApi } from 'main-web';

export function setup(app: PiletApi) {
  app.showNotification('Hello from Piral!', {
    autoClose: 2000,
  });
  app.registerMenu(() =>
    <a href="https://docs.piral.io" target="_blank">Documentation</a>
  );
  app.registerTile(() => <div>Welcome to Piral!</div>, {
    initialColumns: 2,
    initialRows: 1,
  });
  app.registerTile(() => <div>Welcome to Piralaaaa!</div>, {
    initialColumns: 1,
    initialRows: 1,
  });
}
