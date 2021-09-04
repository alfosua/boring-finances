import config from '@/../bfapi.app.config.json'

type Config = {
  baseUrl: string;
};

class BoringFinancesAPI {
  config: Config;

  constructor(config: Config) {
    this.config = config;
  }

  accounts() {
    return new AccountAPI(this.config);
  }

  financialUnits() {
    return new FinancialUnitAPI(this.config);
  }

  operations() {
    return new OperationAPI(this.config);
  }

  request(uri: string, fetchParams: RequestInit) {
    return fetch(`${this.config.baseUrl}/${uri}`, fetchParams);
  }
}

class AccountAPI {
  config: Config;

  constructor(config: Config) {
    this.config = config;
  }

  async all() {
    const uri = `${this.config.baseUrl}/accounts`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }

  async types() {
    const uri = `${this.config.baseUrl}/accounts/types`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }
}

class FinancialUnitAPI {
  config: Config;

  constructor(config: Config) {
    this.config = config;
  }

  async all() {
    const uri = `${this.config.baseUrl}/financial-units`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }

  async types() {
    const uri = `${this.config.baseUrl}/financial-units/types`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }
}

class OperationAPI {
  config: Config;

  constructor(config: Config) {
    this.config = config;
  }

  async all() {
    const uri = `${this.config.baseUrl}/operations`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }
  
  async createOne(operation) {
    const uri = `${this.config.baseUrl}/operations`;
    
    const response = await fetch(uri, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        entries: operation.entries.map(x => x.value),
        notes: operation.entries.map(x => x.value),
      }),
    });
    
    if (response.ok) {
      const result = await response.json();
      return result;
    } else {
      const message = await response.text();
      throw new Error(message);
    }
  }

  entries() {
    return new OperationEntryAPI(this.config);
  }
}

class OperationEntryAPI {
  config: Config;

  constructor(config: Config) {
    this.config = config;
  }

  async types() {
    const uri = `${this.config.baseUrl}/operations/entries/types`;
    const response = await fetch(uri);
    const result = await response.json();

    return result;
  }
}

const singleton = new BoringFinancesAPI(config);

export default singleton;