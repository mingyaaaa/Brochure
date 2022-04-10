<script  lang="ts" setup>
import { reactive, ref, computed } from 'vue'
import { message, notification } from 'ant-design-vue'
import LoginApi from '../api/LoginApi'
const loginApi = new LoginApi();
const CustomeActiveKeys = {
  Account: 'tab1',
  Phone: "tab2",

}
const customActiveKey = ref(CustomeActiveKeys.Account);
const accountLoginErrMsg = ref('');
let isLoginError = false;
const formLogin = ref(null);
const state = {
  loginType: 0, // login type: 0 email, 1 username, 2 telephone
  smsSendBtn: false,
  time: 60,
}
const handleSubmit = () => { };
const handleTabClick = key => {
  isLoginError = false;
  customActiveKey.value = key;
};
const handleUsernameOrEmail = computed((rule, value, callback) => {
  const regex = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/
  if (regex.test(value)) {
    state.loginType = 0
  } else {
    state.loginType = 1
  }
  callback()
});

const getCaptcha = (e) => {
  e.preventDefault()

  const validateFields = formLogin.validateFields;
  validateFields(['mobile'], { force: true }, (err, values) => {
    if (!err) {
      state.smsSendBtn = true

      const interval = window.setInterval(() => {
        if (state.time-- <= 0) {
          state.time = 60
          state.smsSendBtn = false
          window.clearInterval(interval)
        }
      }, 1000)

      const hide = message.loading('验证码发送中..', 0)
      loginApi.getSmsCaptcha({ number: values.mobile }).then(res => {
        setTimeout(hide, 2500)
        notification['success']({
          message: '提示',
          description: '验证码获取成功，您的验证码为：' + res.result.captcha,
          duration: 8
        })
      }).catch(err => {
        setTimeout(hide, 1)
        clearInterval(interval)
        state.time = 60
        state.smsSendBtn = false
        this.requestFailed(err)
      })
    }
  })
}
const requestFailed = error => {
  accountLoginErrMsg.value = error
  isLoginError = true
}
</script>

<template>
  <div class="main">
    <a-form id="formLogin" class="user-layout-login" ref="formLogin" @submit="handleSubmit">
      <a-tabs
        :activeKey="customActiveKey"
        :tabBarStyle="{ textAlign: 'center', borderBottom: 'unset' }"
        @change="handleTabClick"
      >
        <a-tab-pane key="{{CustomeActiveKeys.Account}}" tab="账号密码登录">
          <a-alert
            v-if="isLoginError"
            type="error"
            showIcon
            style="margin-bottom: 24px;"
            :message="accountLoginErrMsg"
          />

          <a-form-item>
            <a-input
              size="large"
              type="text"
              placeholder="账号"
              v-decorator="[
                'account', { initialValue: '', rules: [{ required: true, message: '请输入账号' }, { validator: handleUsernameOrEmail }], validateTrigger: 'change' }
              ]"
            >
              <a-icon slot="prefix" type="user" :style="{ color: 'rgba(0,0,0,.25)' }" />
            </a-input>
          </a-form-item>

          <a-form-item>
            <a-input
              size="large"
              type="password"
              autocomplete="false"
              placeholder="密码"
              v-decorator="[
                'password',
                { initialValue: '', rules: [{ required: true, message: '请输入密码' }], validateTrigger: 'blur' }
              ]"
            >
              <a-icon slot="prefix" type="lock" :style="{ color: 'rgba(0,0,0,.25)' }" />
            </a-input>
          </a-form-item>
        </a-tab-pane>
        <a-tab-pane key="{{CustomeActiveKeys.Phone}}" tab="手机号登录">
          <a-alert
            v-if="isLoginError"
            type="error"
            showIcon
            style="margin-bottom: 24px;"
            :message="accountLoginErrMsg"
          />
          <a-form-item>
            <a-input
              size="large"
              type="text"
              placeholder="手机号"
              v-decorator="['mobile', { rules: [{ required: true, pattern: /^1[34578]\d{9}$/, message: '请输入正确的手机号' }], validateTrigger: 'change' }]"
            >
              <a-icon slot="prefix" type="mobile" :style="{ color: 'rgba(0,0,0,.25)' }" />
            </a-input>
          </a-form-item>

          <a-row :gutter="16">
            <a-col class="gutter-row" :span="16">
              <a-form-item>
                <a-input
                  size="large"
                  type="text"
                  placeholder="验证码"
                  v-decorator="['captcha', { rules: [{ required: true, message: '请输入验证码' }], validateTrigger: 'blur' }]"
                >
                  <a-icon slot="prefix" type="mail" :style="{ color: 'rgba(0,0,0,.25)' }" />
                </a-input>
              </a-form-item>
            </a-col>
            <a-col class="gutter-row" :span="8">
              <a-button
                class="getCaptcha"
                tabindex="-1"
                :disabled="state.smsSendBtn"
                @click.stop.prevent="getCaptcha"
                v-text="!state.smsSendBtn && '获取验证码' || (state.time + ' s')"
              ></a-button>
            </a-col>
          </a-row>
        </a-tab-pane>
      </a-tabs>

      <a-form-item>
        <a-checkbox v-decorator="['rememberMe', { valuePropName: 'checked' }]">记住我</a-checkbox>
        <router-link
          :to="{ name: 'recover', params: { user: 'aaa' } }"
          class="forge-password"
          style="float: right;"
        >忘记密码</router-link>
      </a-form-item>

      <a-form-item>
        <Verify
          @success="verifySuccess"
          :mode="'pop'"
          :captchaType="'clickWord'"
          :imgSize="{ width: '330px', height: '155px' }"
          ref="verify"
        ></Verify>
      </a-form-item>

      <a-form-item style="margin-top:24px">
        <a-button
          size="large"
          type="primary"
          htmlType="submit"
          class="login-button"
          :loading="state.loginBtn"
          :disabled="state.loginBtn"
        >确定</a-button>
      </a-form-item>

      <div class="user-login-other">
        <span>其他登录方式</span>
        <a>
          <a-icon class="item-icon" type="alipay-circle"></a-icon>
        </a>
        <a>
          <a-icon class="item-icon" type="taobao-circle"></a-icon>
        </a>
        <a>
          <a-icon class="item-icon" type="weibo-circle"></a-icon>
        </a>
        <router-link class="register" :to="{ name: 'register' }">注册账户</router-link>
      </div>
    </a-form>

    <two-step-captcha
      v-if="requiredTwoStepCaptcha"
      :visible="stepCaptchaVisible"
      @success="stepCaptchaSuccess"
      @cancel="stepCaptchaCancel"
    ></two-step-captcha>
  </div>
</template>


<style scoped>
a {
  color: #42b983;
}
</style>
